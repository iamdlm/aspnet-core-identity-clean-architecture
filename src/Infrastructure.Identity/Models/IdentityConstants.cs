namespace Infrastructure.Identity.Models
{
    public static class InfrastructureIdentityConstants
    {
        public static class Roles
        {
            public static readonly string Admin = "Admin";
            public static readonly string User = "User";

            public static readonly string[] RolesSupported = { Admin, User };
        }

        public static readonly string DefaultPassword = "Password@1";
    }
}
