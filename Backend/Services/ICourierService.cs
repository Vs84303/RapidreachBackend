using RapidReachNET.DTO;
using RapidReachNET.Models;

namespace RapidReachNET.Services
{
    public interface ICourierService
    {
        Task<Courier> AddCourierAsync(CourierRequestDTO courierDto, long customerId);
        Task<IEnumerable<Courier>> GetAllCouriersAsync();

        Task<Courier?> GetCourierByIdAsync(long id);

        //Task<CourierDto?> GetCourierByIdAsync(long id);

        Task<IEnumerable<Courier>> GetCouriersByUserIdAsync(long userId);

        Task<IEnumerable<CourierDto>> GetCouriersByBranchIdAsync(long branchId);
    }
}
