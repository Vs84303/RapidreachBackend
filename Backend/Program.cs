using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RapidReachNET.Configuration;
using RapidReachNET.DataInitializer;
using RapidReachNET.Models;
using RapidReachNET.Repositories;
using RapidReachNET.Repositories.RapidReachBackend.Repositories;
using RapidReachNET.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null; // <- This removes $id, $values
        options.JsonSerializerOptions.WriteIndented = true;
    });


//  Register DbContext with Pomelo MySQL provider
builder.Services.AddDbContext<RapidreachContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 43)) 
    ));

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JWTSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JWTSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

// Register JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

});

//  Register Services and Repositories
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICourierService, CourierService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHostedService<DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64;  // Optional: increase max depth
});

//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using RapidReachNET.Configuration;
//using RapidReachNET.DataInitializer;
//using RapidReachNET.Models;
//using RapidReachNET.Repositories;
//using RapidReachNET.Repositories.RapidReachBackend.Repositories;
//using RapidReachNET.Services;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// 🔹 CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

//// 🔹 Controllers + JSON options
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = null;
//        options.JsonSerializerOptions.WriteIndented = true;
//        options.JsonSerializerOptions.MaxDepth = 64;
//    });

//// 🔹 EF Core with MySQL
//builder.Services.AddDbContext<RapidreachContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        new MySqlServerVersion(new Version(8, 0, 43))
//    ));

//// 🔹 Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// 🔹 JWT Setup
//var jwtSection = builder.Configuration.GetSection("JWT");
//builder.Services.Configure<JWTSettings>(jwtSection);
//var jwtSettings = jwtSection.Get<JWTSettings>();
//var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = jwtSettings.Issuer,
//        ValidateAudience = true,
//        ValidAudience = jwtSettings.Audience,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key)
//    };
//});

//// 🔹 Services & Repositories
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IBranchService, BranchService>();
//builder.Services.AddScoped<ICourierService, CourierService>();
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//builder.Services.AddScoped<IFeedbackService, FeedbackService>();
//builder.Services.AddScoped<IPaymentService, PaymentService>();
//builder.Services.AddScoped<ITrackingService, TrackingService>();

//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IBranchRepository, BranchRepository>();
//builder.Services.AddScoped<ICourierRepository, CourierRepository>();
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
//builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
//builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();

//builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddHostedService<DataSeeder>();

//var app = builder.Build();

//// 🔹 Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseRouting();
//app.UseCors("AllowAll");
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//// 🔹 Optional: Run EF Core migrations on startup
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<RapidreachContext>();
//    try
//    {
//        db.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("Migration failed: " + ex.Message);
//    }
//}

//app.Run();

