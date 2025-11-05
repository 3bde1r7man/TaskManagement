namespace Domain.Constants
{
    public static class RoleConstants
    {
        // These MUST match exactly what's in your JWT tokens
        public const string Admin = "Admin";
        public const string User = "User";

        // Helper method to get all roles
        public static string[] AllRoles => new[] { Admin, User };
    }
}