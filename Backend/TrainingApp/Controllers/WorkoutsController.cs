using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            var response = _workoutService.GetWorkoutTypes();
            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpGet("progress")]
        public async Task<IActionResult> GetProgress([FromQuery] int year, [FromQuery] int month)
        {
            var response = await _workoutService.GetMonthlyProgress(User.GetUserId(), year, month);
            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllForUser()
        {
            var response = await _workoutService.GetWorkoutsForUser(User.GetUserId());
            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _workoutService.GetWorkoutById(id, User.GetUserId());
            if (!response.Success)
                return NotFound(response.Error);

            return Ok(new { Message = response.Data });
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
