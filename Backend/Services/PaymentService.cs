using RapidReachNET.DTO;
using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICourierRepository _courierRepository;

        public PaymentService(IPaymentRepository paymentRepository, ICourierRepository courierRepository)
        {
            _paymentRepository = paymentRepository;
            _courierRepository = courierRepository;
        }

        public async Task<Payment> AddPaymentAsync(PaymentRequestDTO paymentDto)
        {


            var payment = new Payment
            {
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                CourierId = paymentDto.CourierId
            };

            var addedPayment = await _paymentRepository.AddPaymentAsync(payment);


            return addedPayment;
        }

        public IEnumerable<PaymentDto> GetAllPayments()
        {
            return _paymentRepository.GetAllPayments().Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                CourierId = p.CourierId,

            });
        }

        public IEnumerable<PaymentDto> GetPaymentsByUserId(long userId)
        {
            return _paymentRepository.GetPaymentsByUserId(userId).Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                CourierId = p.CourierId,


            });
        }
    }
}
