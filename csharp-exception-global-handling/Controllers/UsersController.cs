using csharp_exception_global_handling.DTOs;
using csharp_exception_global_handling.Models;
using csharp_exception_global_handling.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_exception_global_handling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("by-email")]
        public async Task<ActionResult<User>> GetUserByEmail([FromQuery] string email)
        {
            _logger.LogInformation("Getting user by email: {Email}", email);
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("Creating new user with email: {Email}", request.Email);
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}/balance")]
        public async Task<ActionResult<User>> UpdateUserBalance(int id, [FromBody] UpdateUserBalanceRequest request)
        {
            _logger.LogInformation("Updating balance for user {UserId} to {NewBalance:C}", id, request.NewBalance);
            var user = await _userService.UpdateUserBalanceAsync(id, request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Deleting user {UserId}", id);
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
