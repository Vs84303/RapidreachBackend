using Microsoft.EntityFrameworkCore;
using RapidReachNET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidReachNET.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly RapidreachContext _context;

        public BranchRepository(RapidreachContext context)
        {
            _context = context;
        }

        public async Task<Branch> AddBranchAsync(Branch branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _context.Branches
                                 .Include(b => b.Users)   // Note: Users plural, collection
                                 .ToListAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(long id)
        {
            return await _context.Branches
                                 .Include(b => b.Users)
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Branch?> GetBranchByEmployeeIdAsync(long employeeId)
        {
            // Find branch where Users collection contains user with matching employeeId
            return await _context.Branches
                                 .Include(b => b.Users)
                                 .FirstOrDefaultAsync(b => b.Users.Any(u => u.UserId == employeeId));
        }
    }
}
