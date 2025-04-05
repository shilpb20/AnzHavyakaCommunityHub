using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IChildService
    {
        Task<Child> CreateChildAsync(Child child);
    }
}