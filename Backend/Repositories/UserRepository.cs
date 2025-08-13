using RapidReachNET.DTO;
using RapidReachNET.Models;
using RapidReachNET.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly RapidreachContext _context;

    public UserRepository(RapidreachContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }


    public async Task<User> UpdateUserAsync(long id, User updatedUser)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null) return null;

        existingUser.UserName = updatedUser.UserName;
        existingUser.Email = updatedUser.Email;
        existingUser.Contact = updatedUser.Contact;
        existingUser.Address = updatedUser.Address;
        existingUser.Pincode = updatedUser.Pincode;
        existingUser.Password = updatedUser.Password;
        existingUser.Role = updatedUser.Role;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public User RegisterUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User UpdateUser(long id, UserDTO updatedUser)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            throw new Exception($"User not found with id: {id}");

        user.UserName = updatedUser.UserName;
        user.Password = updatedUser.Password;
        user.Email = updatedUser.Email;
        user.Contact = updatedUser.Contact;
        user.Pincode = updatedUser.Pincode;
        user.Address = updatedUser.Address;

        _context.SaveChanges();
        return user;
    }

    public IEnumerable<User> GetUsersByRole(string role)
    {
        return _context.Users.Where(u => u.Role == role).ToList();
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
    {
        return await _context.Users
            .Where(u => u.Role == role)
            .ToListAsync();
    }

}
