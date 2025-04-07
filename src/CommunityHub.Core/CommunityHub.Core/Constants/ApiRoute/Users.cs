namespace CommunityHub.Core.Constants
{
    public static partial class ApiRoute
    {
        public static class Users
        {
            /// <summary>Gets all users.</summary>
            public const string GetAll = $"{ApiPrefixes.Users}/all";

            /// <summary>Finds a user by - email or contact or spouse email or spouse contact</summary>
            public const string Find = $"{ApiPrefixes.Users}/find";

            /// <summary>Uploads profile/family picture</summary>
            public const string UploadPicture = $"{ApiPrefixes.Users}/upload/{{id:int}}";
            
        }
    }
}
