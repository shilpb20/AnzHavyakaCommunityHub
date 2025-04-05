namespace CommunityHub.Infrastructure.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
