using Core.Application.DTOs;
using System.Security.Claims;

namespace Core.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(SignInRequest signInRequest);
        Task SignOutAsync();
        Task<AuthenticationResponse> SignUpAsync(SignUpRequest signUpRequest);
        Task<AuthenticationResponse> ChangePasswordAsync(ClaimsPrincipal user, string currentPassword, string newPassword);
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<string> GeneratePasswordResetTokenAsync(ClaimsPrincipal user);
        Task<ApplicationUserDto> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<EmailConfirmationResponse> GenerateEmailConfirmationAsync(ClaimsPrincipal user);
        Task<string> GenerateChangeEmailTokenAsync(ClaimsPrincipal user, string newEmail);
        Task<AuthenticationResponse> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest);
        Task RefreshSignInAsync(ClaimsPrincipal user);        
    }
}
