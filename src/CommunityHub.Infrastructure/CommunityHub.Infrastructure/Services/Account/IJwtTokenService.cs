using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Login;

namespace CommunityHub.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        Task<TokenResponse> GenerateTokenAsync(ApplicationUser user);
    }
}