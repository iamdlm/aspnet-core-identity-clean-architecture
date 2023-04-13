using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using AutoMapper;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Helpers;

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

        public async Task<ApplicationUserDto> FindByIdAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<ApplicationUserDto>(user);
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

        public async Task<AuthenticationResult> ChangeEmailAsync(ApplicationUserDto userDto, string email, string code)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            IdentityResult result = await _userManager.ChangeEmailAsync(user, email, code);
            return result.ToAuthenticationResult();
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<AuthenticationResult> ChangePasswordAsync(ApplicationUserDto userDto, string oldPassword, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<bool> HasPasswordAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.HasPasswordAsync(user);
        }

        public async Task<string> GetEmailAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GetEmailAsync(user);
        }

        public async Task<string> GetUserNameAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<string> GetPhoneNumberAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GetPhoneNumberAsync(user);
        }

        public async Task<AuthenticationResult> SetPhoneNumberAsync(ApplicationUserDto userDto, string phoneNumber)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            IdentityResult result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResult> AddPasswordAsync(ApplicationUserDto userDto, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            IdentityResult result = await _userManager.AddPasswordAsync(user, newPassword);
            return result.ToAuthenticationResult();
        }
    }
}
