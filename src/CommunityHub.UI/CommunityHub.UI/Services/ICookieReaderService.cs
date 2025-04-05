

namespace CommunityHub.UI.Services
{
    public interface ICookieReaderService
    {
        void ClearCookies();
        string GetCookie(string name);
        DateTime GetCookieExpiry(string name);
        Dictionary<string, string> ReadCookiesFromResponse(HttpResponseMessage response);
    }
}