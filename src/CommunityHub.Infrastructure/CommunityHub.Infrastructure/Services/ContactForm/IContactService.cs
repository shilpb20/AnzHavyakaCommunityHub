using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IContactService
    {
        Task<ContactForm> CreateAddUserEnquiryAsync(ContactForm enquiryData);
    }
}
