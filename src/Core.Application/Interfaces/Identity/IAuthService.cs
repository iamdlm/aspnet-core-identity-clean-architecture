using Core.Application.DTOs;
using System.Security.Claims;

namespace Core.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string email, string password, bool rememberMe);
        Task SignOutAsync();
        Task<AuthenticationResult> SignUpAsync(string email, string password);
        Task<AuthenticationResult> ChangePasswordAsync(ApplicationUserDto user, string currentPassword, string newPassword);
        Task<AuthenticationResult> ResetPasswordAsync(ApplicationUserDto user, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUserDto user);
        Task<ApplicationUserDto> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserDto user);
        Task<string> GenerateChangeEmailTokenAsync(ApplicationUserDto user, string newEmail);
        Task<AuthenticationResult> ConfirmEmailAsync(ApplicationUserDto user, string code);
        Task RefreshSignInAsync(ApplicationUserDto user);        
    }
}
