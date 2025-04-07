namespace CommunityHub.Core.Constants
{
    public static partial class ApiRoute
    {
        public static class Admin
        {
            /// <summary>Rejects request by id</summary>
            public const string RejectRequestById = $"{ApiPrefixes.Admin}/registrations/{{id:int}}/reject";

            /// <summary>Gets request by id</summary>
            public const string ApproveRequest = $"{ApiPrefixes.Admin}/registrations/approve";
        }
    }
}
