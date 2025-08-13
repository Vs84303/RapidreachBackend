using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidReachNET.DTO;
using RapidReachNET.Services;
using System;
using System.Threading.Tasks;

namespace RapidReachNET.Controllers
{
    [Route("customer")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _courierService;
        private readonly IUserService _userService;

        public CourierController(ICourierService courierService, IUserService userService)
        {
            _courierService = courierService;
            _userService = userService;
        }

        [HttpPost("addCourier/{customerId}")]
        public async Task<IActionResult> AddCourier(long customerId, [FromBody] CourierRequestDTO courierDto)
        {
            try
            {
                var courier = await _courierService.AddCourierAsync(courierDto, customerId);
                return Ok(new { courierId = courier.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getCourierById/{id}")]
        public async Task<IActionResult> GetCourierById(long id)
        {
            try
            {
                var courier = await _courierService.GetCourierByIdAsync(id);
                if (courier == null)
                    return NotFound();
                return Ok(courier);
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getCouriersByUserId/{userId}")]
        public async Task<IActionResult> GetCouriersByUserId(long userId)
        {
            try
            {
                var couriers = await _courierService.GetCouriersByUserIdAsync(userId);
                return Ok(couriers);
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getCouriersByEmployee/{userId}")]
        public async Task<IActionResult> GetCouriersByUser(long userId)
        {
            try
            {
                // Get branchId for the user
                var branchId = await _userService.GetBranchIdByUserIdAsync(userId);
                if (branchId == null)
                {
                    return NotFound($"User with ID {userId} is not assigned to any branch.");
                }

                // Get couriers for that branch
                var couriersDto = await _courierService.GetCouriersByBranchIdAsync(branchId.Value);

                if (couriersDto == null || !couriersDto.Any())
                {
                    return NotFound($"No couriers found for branch id {branchId}");
                }

                return Ok(couriersDto);
            }
            catch (Exception ex)
            {
                // log ex if needed
                return StatusCode(500, "An error occurred while fetching couriers.");
            }
        }


    }
}
