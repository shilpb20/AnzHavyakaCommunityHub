using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;

namespace CommunityHub.UI.Services.Contact
{
    public class ContactService : BaseService, IContactService
    {
        public ContactService(HttpClient httpClient, 
            IHttpRequestSender requestSender, 
            IOptions<AppSettings> appSettings) : base(httpClient, requestSender, appSettings) { }

        public async Task<ApiResponse<ContactFormDto>> SendUserEnquiryAsync(
            ContactFormCreateDto userEnquiry)
        {
            var result = await AddRequestAsync<ContactFormCreateDto, ContactFormDto>(
                ApiRoute.ContactUs.SubmitEnquiry, userEnquiry);

            return result;
        }
    }
}
