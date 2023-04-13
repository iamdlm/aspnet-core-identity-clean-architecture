using AutoMapper;
using Core.Application.DTOs;
using Infrastructure.Identity.Models;

namespace Infrastructure.Identity.Mapper
{
    public class InfrastructureIdentityProfile : Profile
    {
        public InfrastructureIdentityProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ReverseMap();
        }
    }
}
