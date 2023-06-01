using Core.Application.Interfaces.Application;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureSharedServices(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
        }
    }
}
