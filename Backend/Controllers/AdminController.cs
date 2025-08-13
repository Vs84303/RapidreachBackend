using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidReachNET.DTO;
using RapidReachNET.Models;
using RapidReachNET.Services;
using System;
using System.Threading.Tasks;

namespace RapidReachNET.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeService _userService;
        private readonly IBranchService _branchService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AdminController(IEmployeeService userService, IBranchService branchService, IPasswordHasher<User> passwordHasher)
        {
            _userService = userService;
            _branchService = branchService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("addEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateDTO employeeDto)
        {
            try
            {
                if (employeeDto.BranchId == 0)
                {
                    return BadRequest("BranchId is required for an employee.");
                }

                // Map DTO to User entity
                var user = new User
                {
                    UserName = employeeDto.UserName,
                    Email = employeeDto.Email,
                    Contact = employeeDto.Contact,
                    Address = employeeDto.Address,
                    Pincode = employeeDto.Pincode,
                    BranchId = employeeDto.BranchId,
                    Role = Role.ROLE_EMPLOYEE.ToString()  
                };
                user.Password = _passwordHasher.HashPassword(user, employeeDto.Password);

                var createdUser = await _userService.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding employee: {ex.Message}");
            }
        }


        [HttpPut("updateEmployee/{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] EmployeeUpdateDTO employeeDto)
        {
            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                    return NotFound("Employee not found");

                // Map updated fields from DTO to existing user entity
                existingUser.UserName = employeeDto.UserName;
                existingUser.Email = employeeDto.Email;
                existingUser.Contact = employeeDto.Contact;
                existingUser.Address = employeeDto.Address;
                //existingUser.Password = employeeDto.Password;
                existingUser.Pincode = employeeDto.Pincode;
                existingUser.BranchId = employeeDto.BranchId;
                existingUser.Role = Role.ROLE_EMPLOYEE.ToString(); // Ensure role stays employee

                var updatedUser = await _userService.UpdateUserAsync(id, existingUser);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating employee: {ex.Message}");
            }
        }


        // Get All Users
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
                return StatusCode(500, $"Error retrieving users: {ex.Message}");
            }
        }

        // Get All Employees
        [HttpGet("getAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _userService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving employees: {ex.Message}");
            }
        }

        // Get User by ID
        [HttpGet("getEmployeeById/{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound("Employee not found");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving employee: {ex.Message}");
            }
        }

        // Add Branch
        [HttpPost("addBranch")]
        public async Task<IActionResult> AddBranch([FromBody] Branch branch)
        {
            try
            {
                var createdBranch = await _branchService.AddBranchAsync(branch);
                return CreatedAtAction(nameof(GetBranchById), new { id = createdBranch.Id }, createdBranch);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding branch: {ex.Message}");
            }
        }

        // Get All Branches
        [HttpGet("getAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branches = await _branchService.GetAllBranchesAsync();
                return Ok(branches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving branches: {ex.Message}");
            }
        }

        // Get Branch by ID
        [HttpGet("getBranchById/{id}")]
        public async Task<IActionResult> GetBranchById(long id)
        {
            try
            {
                var branch = await _branchService.GetBranchByIdAsync(id);
                if (branch == null)
                    return NotFound();
                return Ok(branch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving branch: {ex.Message}");
            }
        }
    }
}
