using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}