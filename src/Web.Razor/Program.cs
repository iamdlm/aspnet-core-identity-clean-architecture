using Core.Application.Extensions;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Identity.Extensions;

namespace Web.Razor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Db contexts

            builder.Services.AddAppDbContext(builder.Configuration);

            builder.Services.AddIdentityDbContext(builder.Configuration);

            // Add identity

            builder.Services.AddIdentityAuth(builder.Configuration);

            // Add services

            builder.Services.AddInfrastructureIdentityServices();

            builder.Services.AddInfrastructurePersistenceServices();

            // Add mapping profiles

            builder.Services.AddApplicationMappingProfile();

            builder.Services.AddInfrastructureIdentityMappingProfile();
                        
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.SeedIdentityDataAsync().Wait();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}