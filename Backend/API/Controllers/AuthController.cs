using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [EnableRateLimiting("auth-policy")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var response = await _authService.Register(dto);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new { token= response.Data });
        }

        [HttpPost("login")]
        [EnableRateLimiting("auth-policy")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response = await _authService.Login(dto);

            if (!response.Success)
            {
               return Unauthorized(response.Error);

            }

            return Ok(new {token = response.Data});
        }
    }
}
