using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserAsyncById(int id);
        Task<List<UserInfo>> GetUsersAsync(string? sortBy, bool ascending);

        Task<UserInfo> CreateUserAsync(UserInfo userInfo);
        Task<UserInfo> GetUserInfoByContactNumber(string countryCode, string contactNumber);
        Task<UserInfo> GetUserInfoByEmail(string email);
    }
}