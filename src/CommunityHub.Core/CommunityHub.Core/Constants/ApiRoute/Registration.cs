namespace CommunityHub.Core.Constants
{
    public static partial class ApiRoute
    {
        public static class Registration
        {
            /// <summary>Adds registration requests</summary>
            public const string CreateRequest = $"{ApiPrefixes.Account}/register";

            /// <summary>Retrieves registration requests</summary>
            public const string GetRequests = $"{ApiPrefixes.Account}/register";

            /// <summary>Gets registration requests with id</summary>
            public const string GetRequestById = $"{ApiPrefixes.Account}/register/{{id:int}}";
        }
    }
}
