using RapidReachNET.DTO;
using RapidReachNET.Models;

namespace RapidReachNET.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> UpdateUserAsync(long id, User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long id);

       
        Task<User> UpdateUserAsync(long id, UserDTO userDto);

        Task<long?> GetBranchIdByUserIdAsync(long userId);

        Task<User?> Authenticate(string email, string password);
    }
}
