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


            /// <summary>Deletes child by id</summary>
            public const string DeleteChildById = $"{ApiPrefixes.Users}/delete/child/{{id:int}}";

        }
    }
}
