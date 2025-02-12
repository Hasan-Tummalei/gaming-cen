using System.Text;
using GamingCenter.Application.Interfaces;
using GamingCenter.Application.Interfaces.GamingCenter.Application.Interfaces;
using GamingCenter.Application.Services;
using GamingCenter.Core.Interfaces;
using GamingCenter.Domain.Interfaces;
using GamingCenter.Infrastructure.Configurations;
using GamingCenter.Infrastructure.Data;
using GamingCenter.Infrastructure.Persistance;
using GamingCenter.Infrastructure.Repositories;
using GamingCenter.Infrastructure.Services;
using GamingCenter.Presentation.Endpoints;
using GamingCenter.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GamingCenter.Presentation
{
    /// <summary>
    /// Program class is the entry point for the GamingCenter application. 
    /// It configures services, middleware, and sets up the web application pipeline.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that sets up services, middleware, and runs the web application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddScoped<IJwtService,JwtService>();
            builder.Services.AddScoped<IUserRespository, UserRepository>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<ILoggerService, LoggerService>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IPCRepository, PCRepository>();
            builder.Services.AddScoped<IPCService, PCService>();

            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();


            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("User", policy =>
                {
                    policy.RequireRole("User");
                });
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

                DatabaseInitializer.InitializeAsync(dbContext, passwordHasher).Wait();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapUserEndpoints();
            app.MapPCEndpoints();
            app.MapReservationEndpoints();
            app.MapMembershipEndpoints();
            app.MapOpenApi();
            app.MapGet("/", () => "Hello world");
            app.Run();
        }
    }
}
