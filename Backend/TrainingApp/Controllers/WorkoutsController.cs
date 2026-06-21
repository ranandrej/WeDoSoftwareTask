using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateWorkoutDTO dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            var response = await _workoutService.CreateWorkout(dto, userId);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new { Message = response.Data });
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllForUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            var response = await _workoutService.GetWorkoutsForUser(userId);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new { Message = response.Data });
        }

    }
}

