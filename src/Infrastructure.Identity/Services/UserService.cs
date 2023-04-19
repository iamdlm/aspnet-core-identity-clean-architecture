using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using AutoMapper;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Helpers;
using System.Security.Claims;

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

        public async Task<string> GetUserIdAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<AuthenticationResponse> ChangeEmailAsync(ClaimsPrincipal principal, string email, string code)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            IdentityResult result = await _userManager.ChangeEmailAsync(user, email, code);
            return result.ToAuthenticationResult();
        }

        public async Task<bool> IsEmailConfirmedAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                return false;
            }
            
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<AuthenticationResponse> ChangePasswordAsync(ClaimsPrincipal principal, string oldPassword, string newPassword)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<bool> HasPasswordAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.HasPasswordAsync(user);
        }

        public async Task<string> GetEmailAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.GetEmailAsync(user);
        }

        public async Task<string> GetUserNameAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<string> GetPhoneNumberAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            return await _userManager.GetPhoneNumberAsync(user);
        }

        public async Task<AuthenticationResponse> SetPhoneNumberAsync(ClaimsPrincipal principal, string phoneNumber)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            IdentityResult result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> AddPasswordAsync(ClaimsPrincipal principal, string newPassword)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            IdentityResult result = await _userManager.AddPasswordAsync(user, newPassword);
            return result.ToAuthenticationResult();
        }
    }
}
