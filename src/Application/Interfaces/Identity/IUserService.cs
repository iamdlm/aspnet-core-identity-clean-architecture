using Application.DTOs;
using System.Security.Claims;

namespace Application.Interfaces.Identity
{
    public interface IUserService
    {
        Task<ApplicationUserDto> FindByEmailAsync(string email);
        Task<string> GetUserIdAsync(ApplicationUserDto user);
    }
}
