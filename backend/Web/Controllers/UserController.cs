using Application.UseCases.DeleteUser;
using Application.UseCases.GetUser;
using Application.UseCases.UpdateUser;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IGetUserUseCase _getUser;
        private readonly IUpdateUserUseCase _updateUser;
        private readonly IDeleteUserUseCase _deleteUser;

        public UserController(
            IGetUserUseCase getUser, 
            IUpdateUserUseCase updateUser, 
            IDeleteUserUseCase deleteUser)
        {
            _getUser = getUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _getUser.Handle(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (userId != id.ToString()) return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _updateUser.Handle(id, request);

            if (response is null)
                return BadRequest(new { Message = "Not found a user with this id." });

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "You are not logged in" });
            }

            if (userId != id.ToString()) return Forbid();

            var response = await _deleteUser.Handle(id);

            if (!response)
                return NotFound();

            return Ok();
        }
    }
}
