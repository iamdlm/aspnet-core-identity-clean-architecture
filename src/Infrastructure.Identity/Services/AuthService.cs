using Core.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Core.Application.Interfaces.Identity;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

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

            string userId = _userManager.GetUserId(principal);
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
            ApplicationUser user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);
            
            if (user == null)
            {
                return new AuthenticationResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.UserEmail);

            if (user == null)
            {
                return new AuthenticationResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }
            
            IdentityResult result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token)), request.NewPassword);
            
            return result.ToAuthenticationResult();
        }

        public async Task<TokenResponse> GeneratePasswordResetTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new TokenResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new TokenResponse()
            {
                Succeeded = true,
                Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))
            };
        }

        public async Task<TokenResponse> GenerateEmailConfirmationAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                return new TokenResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return new TokenResponse()
            {
                Succeeded = true,
                UserId = user.Id,
                Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))
            };
        }

        public async Task<AuthenticationResponse> ConfirmEmailAsync(EmailConfirmationRequest request)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return new AuthenticationResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }
            
            string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            
            return result.ToAuthenticationResult();
        }

        public async Task RefreshSignInAsync(ClaimsPrincipal principal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);

            if (user != null)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
        }

        public async Task<TokenResponse> GenerateEmailChangeAsync(ClaimsPrincipal principal, string newEmail)
        {
            ApplicationUser user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                return new TokenResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }

            string token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            
            return new TokenResponse()
            {
                Succeeded = true,
                Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)),
                UserId = user.Id
            };
        }
    }
}
