public static partial class UiRoute
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
}
