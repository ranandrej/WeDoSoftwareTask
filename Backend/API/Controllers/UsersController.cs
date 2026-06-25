using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var response = await _userService.GetCurrentUser(User.GetUserId());
            if (!response.Success)
                return NotFound(response.Error);

            return Ok(new { User = response.Data });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDTO dto)
        {
            if (id != User.GetUserId())
                return Unauthorized(new { Error = "Unauthorized" });

            var response = await _userService.UpdateUser(id, dto);
            if (!response.Success)
                return BadRequest(response.Error);

            return Ok(new { Message = response.Data });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != User.GetUserId())
                return Unauthorized(new { Error = "Unauthorized" });

            var response = await _userService.DeleteUser(id);
            if (!response.Success)
                return NotFound(response.Error);

            return Ok(new { Message = response.Data });
        }
    }
}
