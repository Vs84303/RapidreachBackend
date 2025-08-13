using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;

        public TrackingService(ITrackingRepository trackingRepository)
        {
            _trackingRepository = trackingRepository;
        }

        public async Task<IEnumerable<Tracking>> GetTrackingByCourierIdAsync(long courierId)
        {
            return await _trackingRepository.GetTrackingByCourierIdAsync(courierId);
        }

        public async Task<bool> UpdateTrackingStatusAsync(long trackingId, string newStatus)
        {
            var tracking = await _trackingRepository.GetTrackingByIdAsync(trackingId);

            if (tracking == null)
                return false;

            tracking.TrackingStatus = newStatus;
           // tracking.CourierDate = DateOnly.FromDateTime(DateTime.Now);

            await _trackingRepository.UpdateTrackingAsync(tracking);
            return true;
        }
    }
}
