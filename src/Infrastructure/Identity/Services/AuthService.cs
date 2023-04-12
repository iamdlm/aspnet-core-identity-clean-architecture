using Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Application.Interfaces.Identity;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Helpers;

namespace Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signInManager)
        {
            _mapper = mapper;
            _userManager = UserManager;
            _signInManager = signInManager;
        }

        public async Task<bool> SignInAsync(string email, string password, bool rememberMe)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
            return signInResult.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUserDto> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var userId = _userManager.GetUserId(principal);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<AuthenticationResult> SignUpAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResult> ChangePasswordAsync(ApplicationUserDto userDto, string currentPassword, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResult> ResetPasswordAsync(ApplicationUserDto userDto, string token, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
    }
}
