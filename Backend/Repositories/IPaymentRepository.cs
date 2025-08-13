using RapidReachNET.Models;
using RapidReachNET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace RapidReachNET.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> AddPaymentAsync(Payment payment);
   
            IEnumerable<Payment> GetAllPayments();
            IEnumerable<Payment> GetPaymentsByUserId(long userId);
        
    }
}
