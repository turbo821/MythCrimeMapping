using Application.Dtos;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignUpUserDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        [HttpPost]
        [Route("/code")]
        public async Task<IActionResult> Code([FromBody] string email)
        {
            await _authService.GenerateResetCode(email);
            return Ok("Code send to email");
        }

        [HttpPost]
        [Route("/changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            await _authService.ChangePasswordAsync(dto);
            return Ok("Password changed");
        }
    }
}
