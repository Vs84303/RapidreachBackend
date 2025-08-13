using RapidReachNET.DTO;
using RapidReachNET.Models;

namespace RapidReachNET.Services
{
    public interface IPaymentService
    {
        Task<Payment> AddPaymentAsync(PaymentRequestDTO paymentDto);
        IEnumerable<PaymentDto> GetAllPayments();
        IEnumerable<PaymentDto> GetPaymentsByUserId(long userId);

    }
}
