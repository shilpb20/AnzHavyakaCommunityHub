namespace CommunityHub.Infrastructure.Models.Login
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
