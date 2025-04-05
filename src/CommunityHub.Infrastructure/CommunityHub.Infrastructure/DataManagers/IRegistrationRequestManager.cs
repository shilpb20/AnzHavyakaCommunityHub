using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.DataManagers
{
    public interface IRegistrationRequestManager
    {
        void ApproveRequest(RegistrationRequest request);
        RegistrationRequest CreateRegistrationRequest(string registrationInfo);
        RegistrationRequest CreateRegistrationRequest(RegistrationInfo registrationInfo);

        void RejectRequest(RegistrationRequest request);
    }
}