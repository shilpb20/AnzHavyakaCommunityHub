namespace CommunityHub.UI.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class RedirectToLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectToLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated && context.Request.Path == "/")
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }

        private bool IsAccountPath(string path)
        {
            return path.StartsWith("/Account") || path.StartsWith("/account");
        }

        private bool IsErrorPathPath(string path)
        {
            return path.StartsWith("/Error") || path.StartsWith("/error");
        }
    }
}
