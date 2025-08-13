using RapidReachNET.Models;

namespace RapidReachNET.Services
{
    public interface IEmployeeService
    {

        Task<User> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(long id, User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long id);

        Task<IEnumerable<User>> GetAllEmployeesAsync();

    }
}
