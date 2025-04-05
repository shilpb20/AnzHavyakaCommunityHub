namespace CommunityHub.Core.Constants
{
    public static class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string SuperAdmin = "superadmin";

        public static IEnumerable<string> GetUserRoles()
        {
            return new List<string>()
            {
                User,
                Admin,
                SuperAdmin
            };
        }
    }
}
