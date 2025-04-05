using Microsoft.AspNetCore.Http;

namespace CommunityHub.UI
{
    public static class CacheControlHelper
    {
        public static void SetNoCacheHeaders(HttpResponse response)
        {
            response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0, private";
            response.Headers["Pragma"] = "no-cache";
            response.Headers["Expires"] = "0";
        }
    }
}
