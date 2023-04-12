using AutoMapper;
using Application.DTOs;
using Infrastructure.Identity.Models;

namespace Infrastructure.Mapper
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ReverseMap();
        }
    }
}
