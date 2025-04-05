namespace CommunityHub.Core.Helpers
{
    public static class ApiRouteHelper
    {
        public static string FormatRoute(string route, int id)
        {
            return route.Replace("{id:int}", id.ToString());
        }
    }
}
