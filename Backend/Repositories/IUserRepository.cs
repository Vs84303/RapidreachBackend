using RapidReachNET.DTO;
using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(long userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> RegisterUserAsync(User user);
        Task<User> UpdateUserAsync(long id, User user);
        Task<List<User>> GetAllUsersAsync();

        Task<User?> GetByIdAsync(long userId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

    }
}
