using Application.DTOs;
using Application.Interfaces.Identity;
using AutoMapper;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        public UserManager<ApplicationUser> _userManager { get; }
        public IMapper _mapper { get; }

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApplicationUserDto> FindByEmailAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<string> GetUserIdAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GetUserIdAsync(user);
        }
    }
}
