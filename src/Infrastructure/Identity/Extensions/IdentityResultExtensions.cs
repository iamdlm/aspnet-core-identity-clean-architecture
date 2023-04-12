using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Helpers
{
    public static class IdentityResultExtensions
    {
        public static AuthenticationResult ToAuthenticationResult(this IdentityResult result)
        {
            return new AuthenticationResult()
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.ToDictionary(e => e.Code, e => e.Description)
            };
        }
    }
}
