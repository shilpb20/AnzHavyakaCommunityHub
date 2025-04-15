namespace CommunityHub.Core.Constants
{
    public static partial class ApiRoute
    {
        public static class Account
        {
            /// <summary>Deletes the current user account</summary>
            public const string Delete = $"{ApiPrefixes.Account}/delete-account";

            /// <summary>Registers a new user</summary>
            public const string SetPassword = $"{ApiPrefixes.Account}/set-password";

            /// <summary>
            /// Logs in a user
            /// </summary>
            public const string Login = $"{ApiPrefixes.Account}/login";

            /// <summary>
            /// Logs out a user
            /// </summary>
            public const string Logout = $"{ApiPrefixes.Account}/logout";

            /// <summary>
            /// Sends a password reset email
            /// </summary>
            public const string SendPasswordResetEmail = $"{ApiPrefixes.Account}/send-password-reset-email";

        }
    }
}
