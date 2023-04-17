using Core.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Helpers
{
    public static class IdentityResultExtensions
    {
        public static AuthenticationResponse ToAuthenticationResult(this IdentityResult result)
        {
            return new AuthenticationResponse()
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.ToDictionary(e => e.Code, e => e.Description)
            };
        }
    }
}
