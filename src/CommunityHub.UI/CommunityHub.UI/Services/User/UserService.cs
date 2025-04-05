using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CommunityHub.UI.Services.User
{
    public class UserService : BaseService, IUserService
    {
        public UserService(HttpClient httpClient, IHttpRequestSender requestSender, IOptions<AppSettings> options) : base(httpClient, requestSender, options) { }

        public async Task<ApiResponse<List<UserInfoDto>>> GetAllUsers(string? sortBy, bool ascending)
        {
            string uri = ApiRoute.Users.GetAll;
            if (!string.IsNullOrEmpty(sortBy))
            {
                uri += $"?sortBy={sortBy}&ascending={ascending}";
            }

            return await GetRequestAsync<List<UserInfoDto>>(uri.ToString());
        }
    }
}
