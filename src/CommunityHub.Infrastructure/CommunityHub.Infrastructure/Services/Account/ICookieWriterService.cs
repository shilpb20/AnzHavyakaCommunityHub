namespace CommunityHub.Infrastructure.Services
{
    public interface ICookieWriterService
    {
        void SetCookie(string name, string value, DateTime expiry);
    }
}
