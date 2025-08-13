using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
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

var builder = WebApplication.CreateBuilder(args);

// ========== Render: bind to injected PORT if present ==========
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port) && int.TryParse(port, out var p))
{
    builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(p));
}

// ========== CORS (prod from env, dev fallback AllowAll) ==========
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS"); // e.g. https://your-frontend.onrender.com,https://www.yourdomain.com
builder.Services.AddCors(options =>
{
    if (!string.IsNullOrWhiteSpace(allowedOrigins))
    {
        var origins = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    }
    else
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    }
});

// ========== Controllers & JSON ==========
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = null;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.MaxDepth = 64;
});

// ========== EF Core: auto-pick provider based on env vars ==========
var mssql = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
var mysql = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

if (!string.IsNullOrWhiteSpace(mssql))
{
    // Use SQL Server
    builder.Services.AddDbContext<RapidreachContext>(options =>
        options.UseSqlServer(
            mssql,
            sql => sql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
        )
    );
}
else
{
    // Use MySQL (env or appsettings.json)
    var fallback = string.IsNullOrWhiteSpace(mysql)
        ? builder.Configuration.GetConnectionString("DefaultConnection")
        : mysql;

    builder.Services.AddDbContext<RapidreachContext>(options =>
        options.UseMySql(
            fallback,
            new MySqlServerVersion(new Version(8, 0, 43)),
            mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            }
        )
    );
}

// ========== Swagger ==========
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== JWT (env overrides appsettings; __ = nested keys) ==========
var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JWTSettings>(jwtSection);

var jwtSettings = new JWTSettings
{
    Issuer = Environment.GetEnvironmentVariable("JWT__Issuer") ?? jwtSection["Issuer"],
    Audience = Environment.GetEnvironmentVariable("JWT__Audience") ?? jwtSection["Audience"],
    SecretKey = Environment.GetEnvironmentVariable("JWT__SecretKey") ?? jwtSection["SecretKey"]
};

var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey ?? throw new InvalidOperationException("JWT SecretKey not configured."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // TLS is terminated by Render proxy
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

// ========== Services & Repositories ==========
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

// ========== Reverse proxy headers (before redirects/auth) ==========
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Swagger in prod too (handy on Render)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
