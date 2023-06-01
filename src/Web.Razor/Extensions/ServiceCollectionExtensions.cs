using Core.Application.Interfaces.Application;
using Web.Razor.Services;

namespace Web.Razor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebRazorServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}
