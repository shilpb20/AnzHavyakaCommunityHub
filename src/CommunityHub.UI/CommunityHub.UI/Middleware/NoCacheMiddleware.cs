namespace CommunityHub.UI.Middleware
{
    public class NoCacheMiddleware
    {
        private readonly RequestDelegate _next;

        public NoCacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value?.ToLower();
            if (requestPath.StartsWith("/account") || requestPath.StartsWith("/error"))
            {
                await _next(context);
                return;
            }

            await _next(context);
            if (context.Response.HasStarted || context.Response.StatusCode != 200)
            {
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
            }
        }
    }

}
