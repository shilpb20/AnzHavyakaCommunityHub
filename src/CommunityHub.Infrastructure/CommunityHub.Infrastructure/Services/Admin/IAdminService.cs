using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IAdminService
    {
        Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment);
        Task<UserInfo> ApproveRequestAsync(RegistrationRequest request, string setPasswordUrl);
    }
}