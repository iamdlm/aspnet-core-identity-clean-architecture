using Core.Application.DTOs;

namespace Core.Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<ApplicationUserDto> FindByIdAsync(string userId);
        Task<ApplicationUserDto> FindByEmailAsync(string email);
        Task<string> GetUserIdAsync(ApplicationUserDto user);
        Task<string> GetEmailAsync(ApplicationUserDto user);
        Task<string> GetUserNameAsync(ApplicationUserDto user);
        Task<string> GetPhoneNumberAsync(ApplicationUserDto user);
        Task<AuthenticationResult> ChangeEmailAsync(ApplicationUserDto user, string email, string code);
        Task<AuthenticationResult> ChangePasswordAsync(ApplicationUserDto user, string oldPassword, string newPassword);
        Task<bool> IsEmailConfirmedAsync(ApplicationUserDto user);
        Task<bool> HasPasswordAsync(ApplicationUserDto user);
        Task<AuthenticationResult> SetPhoneNumberAsync(ApplicationUserDto user, string phoneNumber);
        Task<AuthenticationResult> AddPasswordAsync(ApplicationUserDto user, string newPassword);
    }
}
