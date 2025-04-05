
namespace CommunityHub.UI
{
    public interface ICookieStorage
    {
        void ClearCookies();
        string GetCookie(string name);
        bool IsCookiePresent();
        void SetCookie(string name, string value, DateTime expiry);
    }
}