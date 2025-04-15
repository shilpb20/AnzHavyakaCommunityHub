using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Services
{
    public interface IChildService
    {
        Task<List<Child>> GetChildrenByUserInfoIdAsync(int userId);

        Task<Child> CreateChildAsync(Child child);

        Task<Child> DeleteChildAsync(Child child);
    }
}