using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserInfoDto>>> GetAllUsers(string? sortBy, bool ascending);
    }
}