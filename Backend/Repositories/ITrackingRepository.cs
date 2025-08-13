using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{

    public interface ITrackingRepository
    {
        Task AddTrackingAsync(Tracking tracking);

        Task<IEnumerable<Tracking>> GetTrackingByCourierIdAsync(long courierId);

        Task<Tracking?> GetTrackingByIdAsync(long id);
        Task UpdateTrackingAsync(Tracking tracking);
    }
}
