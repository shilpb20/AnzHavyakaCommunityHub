using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Services
{
    public interface IChildService
    {
        Task<Child> CreateChildAsync(Child child);
    }
}