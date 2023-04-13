using Core.Application.Mapper;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationMappingProfile(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new(mc =>
            {
                mc.AddProfile(new ApplicationProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
        }
    }
}
