using RapidReachNET.DTO;
using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{
    public interface ICourierRepository
    {
        Task<Courier> AddCourierAsync(Courier courier);
        Task<IEnumerable<Courier>> GetAllCouriersAsync();
        Task<Courier?> GetCourierByIdAsync(long id);

        Task<IEnumerable<Courier>> GetCouriersByUserIdAsync(long userId);
        Task<IEnumerable<CourierDto>> GetCouriersByBranchIdAsync(long branchId);
    }
}
