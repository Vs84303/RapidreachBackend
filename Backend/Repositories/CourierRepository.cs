using RapidReachNET.Models;

using Microsoft.EntityFrameworkCore;
using RapidReachNET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidReachNET.DTO;

namespace RapidReachNET.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly RapidreachContext _context;

        public CourierRepository(RapidreachContext context)
        {
            _context = context;
        }

        public async Task<Courier> AddCourierAsync(Courier courier)
        {
            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();
            return courier;
        }

        public async Task<IEnumerable<Courier>> GetAllCouriersAsync()
        {
            return await _context.Couriers.ToListAsync();
        }

        public async Task<Courier?> GetCourierByIdAsync(long id)
        {
            return await _context.Couriers
                .Include(c => c.Branch)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Courier>> GetCouriersByUserIdAsync(long userId)
        {
            return await _context.Couriers
                .Where(c => c.UserId == userId)
                .Include(c => c.Branch) 
                .ToListAsync();
        }

        public async Task<IEnumerable<CourierDto>> GetCouriersByBranchIdAsync(long branchId)
        {
            var couriers = await _context.Couriers
                .Where(c => c.BranchId == branchId)
                .Include(c => c.User)
                .Include(c => c.Trackings)
                .Select(c => new CourierDto
                {
                    Id = c.Id,
                    DropLocation = c.DropLocation,
                    ParcelDescription = c.ParcelDescription,
                    ParcelWeight = c.ParcelWeight,
                    PickUpLocation = c.PickUpLocation,
                    Price = c.Price,
                    BranchId = c.BranchId,
                    UserId = c.UserId,

                    User = c.User == null ? null : new UserDtoR
                    {
                        Id = c.User.UserId,
                        UserName = c.User.UserName,
                        Email = c.User.Email,
                        Contact = c.User.Contact,
                    },

                    Trackings = c.Trackings.Select(t => new TrackingDto
                    {
                        Id = t.Id,
                        Status = t.TrackingStatus,
                        UpdatedAt = t.CourierDate,
                    }).ToList()
                })
                .ToListAsync();

            return couriers;
        }


    }
}
