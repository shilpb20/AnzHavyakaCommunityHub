using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services.Account
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(Login model);
        Task<ApiResponse<bool>> SetNewPasswordAsync(SetPassword model);
        Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string email);
    }
}