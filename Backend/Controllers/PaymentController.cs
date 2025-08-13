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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("addPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequestDTO paymentDto)
        {
            try
            {
                var payment = await _paymentService.AddPaymentAsync(paymentDto);
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Error adding payment.");
            }
        }
        [HttpGet("GetAllPayments")]
        public IActionResult GetAllPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("GetPaymentsByUserId/{userId}")]
        public IActionResult GetPaymentsByUserId(long userId)
        {
            var payments = _paymentService.GetPaymentsByUserId(userId);
            return Ok(payments);
        }
    }
}
