using RapidReachNET.Models;
using RapidReachNET.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RapidReachNET.DataInitializer
{
    public class DataSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

                try
                {
                    var existingAdmin = await userRepository.GetUserByEmailAsync("admin@gmail.com");
                    if (existingAdmin == null)
                    {
                        var adminUser = new User
                        {
                            UserName = "Admin",
                            Email = "admin@gmail.com",
                            Contact = "9876567898",
                            Pincode = "4150032",
                            Address = "Mumbai, India",
                            Role = "ROLE_ADMIN"
                        };

                        adminUser.Password = passwordHasher.HashPassword(adminUser, "admin@123");

                        await userRepository.RegisterUserAsync(adminUser);
                        Console.WriteLine("Admin user added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Admin user already exists.");
                    }
                }
                catch (Exception ex)
                {
                    // Do not crash the app if DB is unreachable on startup
                    Console.WriteLine($"Data seeding skipped due to DB error: {ex.Message}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
