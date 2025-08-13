using Microsoft.EntityFrameworkCore;
using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly RapidreachContext _context;

        public TrackingRepository(RapidreachContext context)
        {
            _context = context;
        }

        public async Task AddTrackingAsync(Tracking tracking)
        {
            _context.Trackings.Add(tracking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tracking>> GetTrackingByCourierIdAsync(long courierId)
        {
            return await _context.Trackings
                .Where(t => t.CourierId == courierId)
                .ToListAsync();
        }

        public async Task<Tracking?> GetTrackingByIdAsync(long id)
        {
            return await _context.Trackings.FindAsync(id);
        }

        public async Task UpdateTrackingAsync(Tracking tracking)
        {
            _context.Trackings.Update(tracking);
            await _context.SaveChangesAsync();
        }
    }
}
