using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;

namespace CommunityHub.UI.Services.Registration
{
    public class RegistrationService : BaseService, IRegistrationService
    {
        public RegistrationService(HttpClient httpClient, IHttpRequestSender requestSender, IOptions<AppSettings> options) : base(httpClient, requestSender, options) { }

        public async Task<ApiResponse<RegistrationRequestDto>> SendRegistrationRequestAsync(
            RegistrationInfoCreateDto registrationData)
        {
            var result = await AddRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(
                ApiRoute.Registration.CreateRequest, registrationData);

            return result;
        }
    }
}
