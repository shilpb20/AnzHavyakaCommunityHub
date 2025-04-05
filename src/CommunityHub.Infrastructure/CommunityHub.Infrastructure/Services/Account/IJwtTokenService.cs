using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        Task<TokenResponse> GenerateTokenAsync(ApplicationUser user);
    }
}