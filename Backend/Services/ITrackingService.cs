using RapidReachNET.Models;

namespace RapidReachNET.Services
{
    public interface ITrackingService
    {

        Task<IEnumerable<Tracking>> GetTrackingByCourierIdAsync(long courierId);

        Task<bool> UpdateTrackingStatusAsync(long trackingId, string newStatus);
    }
}
