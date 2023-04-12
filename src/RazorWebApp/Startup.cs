using Application.Mapper;
using AutoMapper;
using Infrastructure.Mapper;

namespace RazorWebApp
{
    public static class Startup
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new(mc =>
            {
                mc.AddProfile(new ApplicationProfile());
                mc.AddProfile(new InfrastructureProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
        }
    }
}
