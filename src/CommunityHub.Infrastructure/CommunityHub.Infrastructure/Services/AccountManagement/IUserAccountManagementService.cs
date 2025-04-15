
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Services
{
    public interface IUserAccountManagementService
    {
        Task<UserInfo> CreateUserAccountAsync(RegistrationInfo registrationInfo);
        Task<bool> DeleteUserAccountAsync(int userId);
    }
}