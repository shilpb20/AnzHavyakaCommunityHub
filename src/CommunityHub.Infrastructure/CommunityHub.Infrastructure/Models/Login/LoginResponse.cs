namespace CommunityHub.Infrastructure.Models.Login
{
    public class LoginResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }

        public TokenResponse TokenResponse { get; set; }
        public DateTime TokenExpiration { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
