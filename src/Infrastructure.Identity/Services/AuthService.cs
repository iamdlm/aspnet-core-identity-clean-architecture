using Core.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Core.Application.Interfaces.Identity;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Helpers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IMapper mapper,
            UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _userManager = UserManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest loginRequest)
        {
            // Authenticate the user using the provided email and password
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, loginRequest.RememberMe, false);

            if (signInResult.Succeeded)
            {
                // Retrieve the user from the UserManager
                ApplicationUser user = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (user == null)
                {
                    return new TokenResponse
                    {
                        Succeeded = false,
                        Errors = new Dictionary<string, string>() { { string.Empty, "Invalid login attempt." } }
                    };
                }

                // Create claims for the token
                string role = (await _userManager.GetRolesAsync(user))[0];

                // Generate a new authentication token
                string authToken = TokenHelper.GenerateAuthToken(
                    TokenHelper.GenerateClaims(user.Id, user.UserName, role),
                    _jwtSettings.Secret,
                    _jwtSettings.DurationInMinutes);

                // Generate refresh token
                string refreshToken = TokenHelper.GenerateRefreshToken();

                // Save refresh token to user
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshDurationInMinutes);
                await _userManager.UpdateAsync(user);

                // Create the SignInResponse object to return
                TokenResponse signInResponse = new TokenResponse
                {
                    Succeeded = true,
                    AccessToken = authToken,
                    RefreshToken = refreshToken
                };

                return signInResponse;
            }

            return new TokenResponse
            {
                Succeeded = false,
                Errors = new Dictionary<string, string>() { { string.Empty, "Invalid login attempt." } }
            };
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

        public async Task<TokenResponse> SignUpAsync(SignUpRequest signUpRequest)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = signUpRequest.Email,
                UserName = signUpRequest.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, signUpRequest.Password);

            if (result.Succeeded)
            {
                // Add default "User" role
                await _userManager.AddToRoleAsync(user, InfrastructureIdentityConstants.Roles.User);

                await _signInManager.SignInAsync(user, isPersistent: false);

                // Create claims for the token
                string role = (await _userManager.GetRolesAsync(user))[0];

                // Generate a new authentication token
                var authToken = TokenHelper.GenerateAuthToken(
                    TokenHelper.GenerateClaims(user.Id, user.UserName, role),
                    _jwtSettings.Secret,
                    _jwtSettings.DurationInMinutes);

                // Generate refresh token
                var refreshToken = TokenHelper.GenerateRefreshToken();

                // Save refresh token to user
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshDurationInMinutes);
                await _userManager.UpdateAsync(user);

                // Create the SignInResponse object to return
                return new TokenResponse
                {
                    Succeeded = true,
                    AccessToken = authToken,
                    RefreshToken = refreshToken
                };
            }

            return new TokenResponse
            {
                Succeeded = false,
                Errors = result.Errors.ToDictionary(error => error.Code, error => error.Description)
            };
        }

        public async Task<AuthenticationResponse> ChangePasswordAsync(ApplicationUserDto userDto, string currentPassword, string newPassword)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.ToAuthenticationResult();
        }

        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(resetPasswordRequest.UserEmail);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);
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

        public async Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(request.UserId);
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, request.Token);
            return result.ToAuthenticationResult();
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

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            ClaimsPrincipal principal = TokenHelper.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken, _jwtSettings.Secret);
            string userName = principal.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
            {
                return new TokenResponse
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid refresh token." } }
                };
            }

            // Generate a new authentication token
            var authToken = TokenHelper.GenerateAuthToken(
                principal.Claims,
                _jwtSettings.Secret,
                _jwtSettings.DurationInMinutes);

            // Generate a new refresh token
            var newRefreshToken = TokenHelper.GenerateRefreshToken();

            // Save the new refresh token to the user
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshDurationInMinutes);
            await _userManager.UpdateAsync(user);

            // Create the SignInResponse object to return
            TokenResponse signInResponse = new()
            {
                Succeeded = true,
                AccessToken = authToken,
                RefreshToken = newRefreshToken
            };

            return signInResponse;
        }

        public async Task<AuthenticationResponse> RevokeTokenAsync(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return new AuthenticationResponse()
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string>() { { string.Empty, "Invalid request." } }
                };
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return new AuthenticationResponse() { Succeeded = true };
        }
    }
}
