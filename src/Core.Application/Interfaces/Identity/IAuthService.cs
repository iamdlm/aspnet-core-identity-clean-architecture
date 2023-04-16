using Core.Application.DTOs;
using System.Security.Claims;

namespace Core.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<TokenResponse> SignUpAsync(SignUpRequest signUpRequest);
        Task<TokenResponse> SignInAsync(SignInRequest signInRequest);
        Task SignOutAsync();
        Task<AuthenticationResponse> ChangePasswordAsync(ApplicationUserDto user, string currentPassword, string newPassword);
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUserDto user);
        Task<ApplicationUserDto> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserDto user);
        Task<string> GenerateChangeEmailTokenAsync(ApplicationUserDto user, string newEmail);
        Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        Task RefreshSignInAsync(ApplicationUserDto user);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        Task<AuthenticationResponse> RevokeTokenAsync(string userName);
    }
}
