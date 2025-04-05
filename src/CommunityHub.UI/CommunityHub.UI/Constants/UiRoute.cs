
using CommunityHub.Core.Constants;

public static class UiRoute
{
    public static class Account
    {
        public const string Register = $"{UiRoutePrefix.Account}/register/user-request";
        public const string SetPassword = $"{UiRoutePrefix.Account}/set-password";

        public const string ForgotPassword = $"{UiRoutePrefix.Account}/forgot-password";
        public const string ResetPassword = $"{UiRoutePrefix.Account}/reset-password";

        public const string Login = $"{UiRoutePrefix.Account}/login";
        public const string Logout = $"{UiRoutePrefix.Account}/logout";
    }

    public static class Admin
    {
        public const string Index = $"{UiRoutePrefix.Admin}/index";
        public const string RegistrationRequest = $"{UiRoutePrefix.Admin}/user-requests";
        public const string ApproveRequest = $"{UiRoutePrefix.Admin}/user-requests/accept";
        public const string RejectRequest = $"{UiRoutePrefix.Admin}/user-requests/reject";
    }

    public static class Home
    {
        public const string Index = $"{UiRoutePrefix.Home}/index";
        public const string Privacy = $"{UiRoutePrefix.Home}/privacy";
    }


    public static class Error
    {
        public const string ByStatusCode = $"{UiRoutePrefix.Error}/status/{{statusCode}}";
        public const string GeneralError = $"{UiRoutePrefix.Error}";
        public const string PageNotFound = $"{UiRoutePrefix.Error}/status/404";
    }
}
