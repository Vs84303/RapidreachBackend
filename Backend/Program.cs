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

// ---------- Render: optional PORT binding (works even if ASPNETCORE_URLS is not set) ----------
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port) && int.TryParse(port, out var p))
{
    builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(p));
}

// ---------- CORS: allow from env (comma separated) or fall back to AllowAll for dev ----------
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
        // Dev fallback
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    }
});

// ---------- Controllers + sane JSON options (no duplicate AddControllers) ----------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Avoid $id/$ref noise and keep responses pretty
    options.JsonSerializerOptions.ReferenceHandler = null;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.MaxDepth = 64;
});

// ---------- EF Core with MySQL (Render: env var override) ----------
var csFromEnv = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
var connectionString = string.IsNullOrWhiteSpace(csFromEnv)
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : csFromEnv;

builder.Services.AddDbContext<RapidreachContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 43)))
);

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- JWT from configuration or env (Render uses double-underscore for nested keys) ----------
var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JWTSettings>(jwtSection);

var jwtSettings = new JWTSettings
{
    Issuer = Environment.GetEnvironmentVariable("JWT__Issuer") ?? jwtSection["Issuer"],
    Audience = Environment.GetEnvironmentVariable("JWT__Audience") ?? jwtSection["Audience"],
    SecretKey = Environment.GetEnvironmentVariable("JWT__SecretKey") ?? jwtSection["SecretKey"]
};

var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey ?? throw new InvalidOperationException("JWT SecretKey not configured."));

// ---------- Authentication ----------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Render terminates SSL at the proxy; we honor X-Forwarded-Proto below
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

// ---------- Services & Repos ----------
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

// ---------- Render: respect reverse proxy headers BEFORE redirects/auth ----------
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// (Optional) enable Swagger in production too (handy on Render)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();        // safe because X-Forwarded-Proto is honored above
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
