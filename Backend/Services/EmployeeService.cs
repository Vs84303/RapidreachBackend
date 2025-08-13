using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{
    

        public class EmployeeService : IEmployeeService
        {
            private readonly IEmployeeRepository _userRepository;

            public EmployeeService(IEmployeeRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<User> AddUserAsync(User user)
            {
            user.Role=Role.ROLE_EMPLOYEE.ToString();
                return await _userRepository.AddUserAsync(user);
            }

            public async Task<User?> UpdateUserAsync(long id, User user)
            {
                var existingUser = await _userRepository.GetUserByIdAsync(id);
                if (existingUser == null) return null;

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Contact = user.Contact;
                existingUser.Pincode = user.Pincode;
                existingUser.Address = user.Address;
                existingUser.Password = user.Password;
          //  user.Password = _passwordHasher.HashPassword(user, userDto.Password);
            existingUser. Role = Role.ROLE_EMPLOYEE.ToString();

                return await _userRepository.UpdateUserAsync(existingUser);
            }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _userRepository.GetAllUsersAsync();
            }

            public async Task<User?> GetUserByIdAsync(long id)
            {
                return await _userRepository.GetUserByIdAsync(id);
            }

        public async Task<IEnumerable<User>> GetAllEmployeesAsync()
        {
            return await _userRepository.GetAllEmployeesAsync();
        }

    }
}
