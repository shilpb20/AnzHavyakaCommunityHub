using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;

namespace CommunityHub.UI.Services.Account
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(HttpClient httpClient, IHttpRequestSender requestSender, IOptions<AppSettings> options) : base(httpClient, requestSender, options) { }


        public async Task<ApiResponse<bool>> SetNewPasswordAsync(SetPassword model)
        {
            return await _requestSender.SendPostRequestAsync<SetPassword, bool>(_httpClient, ApiRoute.Account.SetPassword, model);
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(Login model)
        {
            return await _requestSender.SendPostRequestAsync<Login, LoginResponseDto>(_httpClient, ApiRoute.Account.Login, model);
        }

        public async Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string email)
        {
            string baseAddress = _appSettings.SiteUrl.TrimEnd('/'); ;
            var url = $"{baseAddress}{UiRoute.Account.ResetPassword}";

            var emailLink = new PasswordLink() { Email = email, Url = url };
            return await _requestSender.SendPostRequestAsync<PasswordLink, bool>(_httpClient, ApiRoute.Account.SendPasswordResetEmail, emailLink);
        }
    }
}
