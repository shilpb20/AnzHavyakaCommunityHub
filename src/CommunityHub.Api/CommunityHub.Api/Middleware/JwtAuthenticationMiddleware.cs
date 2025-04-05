using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CommunityHub.Api.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;

        public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Authentication failed: {Message}", result.Failure?.Message);
            }
            else
            {
                _logger.LogInformation("Authentication succeeded for user: {User}", result.Principal?.Identity?.Name);
                context.User = result.Principal;
            }

            await _next(context);
        }
    }

}
