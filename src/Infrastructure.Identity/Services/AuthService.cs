using Core.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Core.Application.Interfaces.Identity;
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

        public async Task<bool> SignInAsync(SignInRequest request)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);
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

        public async Task<AuthenticationResponse> SignUpAsync(SignUpRequest request)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> ChangePasswordAsync(ApplicationUserDto userDto, string currentPassword, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.UserEmail);
            
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
                return result.ToAuthenticationResult();
            }

            return new AuthenticationResponse()
            {
                Succeeded = false,
                Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
            };
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user != null)
            {
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, request.Token);
                return result.ToAuthenticationResult();
            }

            return new AuthenticationResponse()
            {
                Succeeded = false,
                Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
            };
        }

        public async Task RefreshSignInAsync(ApplicationUserDto userDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task<string> GenerateChangeEmailTokenAsync(ApplicationUserDto userDto, string newEmail)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);

            return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        }
    }
}
