using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services.Contact
{
    public interface IContactService
    {
        Task<ApiResponse<ContactFormDto>> SendUserEnquiryAsync(ContactFormCreateDto userEnquiry);
    }
}