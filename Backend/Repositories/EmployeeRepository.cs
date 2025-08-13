namespace RapidReachNET.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using RapidReachNET.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace RapidReachBackend.Repositories
    {
        public class EmployeeRepository : IEmployeeRepository
        {
            private readonly RapidreachContext _context;

            public EmployeeRepository(RapidreachContext context)
            {
                _context = context;
            }

            public async Task<User> AddUserAsync(User user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<User> UpdateUserAsync(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _context.Users.ToListAsync();
            }

            public async Task<User?> GetUserByIdAsync(long id)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            }

            public async Task<IEnumerable<User>> GetAllEmployeesAsync()
            {
                return await _context.Users
                    .Where(u => u.Role == Role.ROLE_EMPLOYEE.ToString())
                    .ToListAsync();
            }

        }
    }

}
