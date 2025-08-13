using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{
    public interface IEmployeeRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);

        Task<IEnumerable<User>> GetAllEmployeesAsync();

    }
}
