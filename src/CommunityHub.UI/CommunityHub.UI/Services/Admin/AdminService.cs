using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.UI.Controllers;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace CommunityHub.UI.Services.Admin
{
    public class AdminService : BaseService, IAdminService
    {
        public AdminService(HttpClient httpClient, IHttpRequestSender requestSender, IOptions<AppSettings> options) : base(httpClient, requestSender, options) { }

        public async Task<ApiResponse<List<RegistrationRequestDto>>> GetPendingRequests()
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Registration.Status, RegistrationStatusHelper.PendingStatus }
            };

            string uri = HttpHelper.BuildUri(_httpClient.BaseAddress.ToString(), ApiRoute.Registration.GetRequests, queryParameters);
            var result = await _requestSender.SendGetRequestAsync<List<RegistrationRequestDto>>(_httpClient, uri);
            return result;
        }

        public async Task<ApiResponse<RegistrationRequestDto>> RejectRegistrationRequest(int id, string rejectionReason)
        {
            var result = await _requestSender.SendUpdateRequestAsync<string, RegistrationRequestDto>(_httpClient,
                ApiRoute.Admin.RejectRequestById, id, rejectionReason);

            return result;
        }

        public async Task<ApiResponse<UserInfoDto>> ApproveRegistrationRequest(int id, string appUrl)
        {
            ApproveRequest request = new ApproveRequest()
            {
                Id = id,
                SetPasswordUrl = appUrl
            };

            var result = await _requestSender.SendPostRequestAsync<ApproveRequest, UserInfoDto>(_httpClient, ApiRoute.Admin.ApproveRequest, request);
            return result;
        }
    }
}
