using Microsoft.AspNetCore.Http;

namespace CommunityHub.Infrastructure.Services
{
    public class CookieWriterService : ICookieWriterService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieWriterService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string name, string value, DateTime expiry)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value, new CookieOptions
            {
                Expires = expiry,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
        }
    }
}
