using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Reporting;
using QuestPDF.Fluent;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly IUserService _userService;

        public WorkoutsController(IWorkoutService workoutService, IUserService userService)
        {
            _workoutService = workoutService;
            _userService = userService;
        }

        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            var response = _workoutService.GetWorkoutTypes();
            return Ok(new { Types = response.Data });
        }

        [Authorize]
        [HttpGet("progress")]
        public async Task<IActionResult> GetProgress([FromQuery] int year, [FromQuery] int month)
        {
            var response = await _workoutService.GetMonthlyProgress(User.GetUserId(), year, month);
            if (!response.Success)
                return BadRequest(response.Error);
            if (response.Data is null)
                return BadRequest("Progress data not available");
            return Ok(new { Progress = response.Data });
        }

        [Authorize]
        [HttpGet("progress-pdf")]
        public async Task<IActionResult> GetProgressPdf([FromQuery] int year, [FromQuery] int month)
        {
            var userId = User.GetUserId();
            var response = await _workoutService.GetMonthlyProgress(userId, year, month);
            if (!response.Success)
                return BadRequest(response.Error);
            if (response.Data is null)
                return BadRequest("Progress data not available");
            var userResponse = await _userService.GetCurrentUser(userId);
            if (!userResponse.Success || userResponse.Data is null)
                return BadRequest(userResponse.Error ?? "User not found");
            var document = new MonthlyProgressDocument(
                response.Data,
                userResponse.Data.Name,
                userResponse.Data.Surename
            );
            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"progress-{month}-{year}.pdf");
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllForUser(
            [FromQuery] int year,
            [FromQuery] int month,
            [FromQuery] string sort = "desc")
        {
            var descending = !string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase);
            var response = await _workoutService.GetWorkoutsForUser(User.GetUserId(), year, month, descending);
            if (!response.Success)
                return BadRequest(response.Error);
            if (response.Data is null)
                return BadRequest("Workouts not available");
            return Ok(new { Workouts = response.Data });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _workoutService.GetWorkoutById(id, User.GetUserId());
            if (!response.Success)
                return NotFound(response.Error);
            if (response.Data is null)
                return NotFound("Workout not found");

            return Ok(new { Workout = response.Data });
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateWorkoutDTO dto)
        {
            var response = await _workoutService.CreateWorkout(dto, User.GetUserId());
            if (!response.Success)
                return BadRequest(response.Error);
            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateWorkoutDTO dto)
        {
            var response = await _workoutService.UpdateWorkout(id, dto, User.GetUserId());
            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _workoutService.DeleteWorkout(id, User.GetUserId());
            if (!response.Success)
                return NotFound(response.Error);
            return Ok(new { Message = response.Data });
        }
    }
}
