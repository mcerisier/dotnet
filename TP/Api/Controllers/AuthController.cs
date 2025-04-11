using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(new { Errors = result.Errors });

            return Ok(new
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            if (!result.Success)
                return Unauthorized(new { Errors = result.Errors });

            return Ok(new
            {
                Token = result.Token
            });
        }

        [HttpGet("/admin/crash")]
        [Authorize(Roles = "admin")]
        public IActionResult Crash()
        {
            throw new Exception("Test crash");
        }
    }
}
