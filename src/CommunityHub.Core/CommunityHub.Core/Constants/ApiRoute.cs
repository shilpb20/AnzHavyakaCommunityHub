namespace CommunityHub.Core.Constants
{
    public static class ApiRoute
    {
        public static class Account
        {
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

        public static class Users
        {
            /// <summary>Gets all users.</summary>
            public const string GetAll = $"{ApiPrefixes.Users}/all";

            /// <summary>Finds a user by - email or contact or spouse email or spouse contact</summary>
            public const string Find = $"{ApiPrefixes.Users}/find";

            /// <summary>Uploads profile/family picture</summary>
            public const string UploadPicture = $"{ApiPrefixes.Users}/upload/{{id:int}}";
            
        }

        public static class Registration
        {
            /// <summary>Adds registration requests</summary>
            public const string CreateRequest = $"{ApiPrefixes.Account}/register";

            /// <summary>Retrieves registration requests</summary>
            public const string GetRequests = $"{ApiPrefixes.Account}/register";

            /// <summary>Gets registration requests with id</summary>
            public const string GetRequestById = $"{ApiPrefixes.Account}/register/{{id:int}}";
        }

        public static class Admin
        {
            /// <summary>Rejects request by id</summary>
            public const string RejectRequestById = $"{ApiPrefixes.Admin}/registrations/{{id:int}}/reject";

            /// <summary>Gets request by id</summary>
            public const string ApproveRequest = $"{ApiPrefixes.Admin}/registrations/approve";
        }

        public static class FamilyPicture
        {
            /// <summary>Gets all family pictures</summary>
            public const string GetAll = $"{ApiPrefixes.FamilyPictures}/all";

            /// <summary>Get/Update/Delete family picture by id</summary>
            public const string ById = $"{ApiPrefixes.FamilyPictures}/{{id:int}}";
        }
    }
}
