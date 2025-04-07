public static partial class UiRoute
{
    public static class Admin
    {
        public const string Index = $"{UiRoutePrefix.Admin}/index";
        public const string RegistrationRequest = $"{UiRoutePrefix.Admin}/user-requests";
        public const string ApproveRequest = $"{UiRoutePrefix.Admin}/user-requests/accept";
        public const string RejectRequest = $"{UiRoutePrefix.Admin}/user-requests/reject";
    }
}
