using Core.Application.Extensions;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Identity.Extensions;

namespace Web.Api
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

            builder.Services.AddIdentityAuth();

            // Add services

            builder.Services.AddInfrastructureIdentityServices();

            builder.Services.AddInfrastructurePersistenceServices();

            // Add mapping profiles

            builder.Services.AddApplicationMappingProfile();

            builder.Services.AddInfrastructureIdentityMappingProfile();

            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}