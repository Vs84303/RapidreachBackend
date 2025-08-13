using RapidReachNET.DTO;
using RapidReachNET.Models;
using RapidReachNET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidReachNET.Models;

namespace RapidReachNET.Controllers
{
    [Route("customer")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserController(IUserService userService,  IPasswordHasher<User> passwordHasher)
        {
            _userService = userService;
           
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Password))
                    return BadRequest("Password is required.");

                user.Role = "ROLE_CUSTOMER";
                var newUser = await _userService.RegisterUserAsync(user);
                return Created("", newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error registering user: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] User user)
        {
            try
            {
                user.Role = "ROLE_CUSTOMER";

                var updatedUser = await _userService.UpdateUserAsync(id, user);
                if (updatedUser == null)
                    return NotFound("User not found");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching users: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching user: {ex.Message}");
            }
        }

       
    }
}
