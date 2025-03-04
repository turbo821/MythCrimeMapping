using Application.Dtos;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("code")]
        public async Task<IActionResult> Code([FromBody] Guid userId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid || id != userId.ToString())
                return BadRequest(ModelState);

            await _authService.GenerateResetCode(userId);
            return Ok("Code send to email");
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (!ModelState.IsValid || userId != dto.UserId.ToString())
                return BadRequest(ModelState);

            await _authService.ChangePasswordAsync(dto);
            return Ok("Password successfull changed");
        }
    }
}
