using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData, string siteUrl);
        Task<RegistrationRequest> GetRequestByIdAsync(int id);
        Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status = eRegistrationStatus.Pending);
        Task<RegistrationRequest> UpdateRequestAsync(RegistrationRequest registrationRequest);
    }
}
