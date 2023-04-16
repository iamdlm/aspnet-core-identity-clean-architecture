using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Application.Interfaces.Identity;
using Infrastructure.Identity.Services;
using AutoMapper;
using Infrastructure.Identity.Mapper;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));
        }

        public static void AddIdentityAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
                };
            });
        }

        public static void AddInfrastructureIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddInfrastructureIdentityMappingProfile(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new(mc =>
            {
                mc.AddProfile(new InfrastructureIdentityProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
        }

        public static async Task SeedIdentityDataAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Add roles supported
                await roleManager.CreateAsync(new IdentityRole(InfrastructureIdentityConstants.Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(InfrastructureIdentityConstants.Roles.User));

                // New admin user
                string adminUserName = "admin@mail.com";
                var adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminUserName,
                    EmailConfirmed = true,
                    FirstName = "Admin"
                };

                // Add new user and their role
                await userManager.CreateAsync(adminUser, InfrastructureIdentityConstants.DefaultPassword);
                adminUser = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(adminUser, InfrastructureIdentityConstants.Roles.Admin);
            }
        }
    }
}
