using RapidReachNET.Models;


using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RapidReachNET.DTO;
namespace RapidReachNET.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly RapidreachContext _context;

        public PaymentRepository(RapidreachContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _context.Payments
                .Include(p => p.Courier)
                .ToList();
        }

        public IEnumerable<Payment> GetPaymentsByUserId(long userId)
        {
            return _context.Payments
                .Include(p => p.Courier)
                .Where(p => p.Courier.UserId == userId) // Assuming Courier has UserId
                .ToList();
        }
    }
}
