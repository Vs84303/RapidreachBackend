using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidReachNET.Models;
using RapidReachNET.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RapidReachNET.Controllers
{
    [Route("customer")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpGet("getTrackingStatusByCourierId/{courierId}")]
        public async Task<IActionResult> GetTrackingStatusByCourierId(long courierId)
        {
            try
            {
                var trackings = await _trackingService.GetTrackingByCourierIdAsync(courierId);
                if (trackings == null || !trackings.Any())
                    return NotFound("No tracking found for this courier ID.");

                return Ok(trackings);
            }
            catch (Exception ex)
            {
                // log error here if logging is setup
                return StatusCode(500, $"Error retrieving tracking status: {ex.Message}");
            }
        }

        
        [HttpPut("updateStatus/{trackingId}")]
        public async Task<IActionResult> UpdateTrackingStatus(long trackingId, [FromBody] string newStatus)
        {
            try
            {
                var success = await _trackingService.UpdateTrackingStatusAsync(trackingId, newStatus);
                if (!success)
                    return NotFound("Tracking record not found.");

                return Ok("Tracking status updated successfully.");
            }
            catch (Exception ex)
            {
                // log error here if logging is setup
                return StatusCode(500, $"Error updating tracking status: {ex.Message}");
            }
        }
    }
}
