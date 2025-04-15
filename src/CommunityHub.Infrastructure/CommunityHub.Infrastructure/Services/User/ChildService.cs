using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.Services
{
    public class ChildService : IChildService
    {
        private readonly ILogger<ChildService> _logger;
        private readonly IRepository<Child, ApplicationDbContext> _childRepository;

        public ChildService(ILogger<ChildService> logger,
            IRepository<Child, ApplicationDbContext> childRepository)
        {
            _logger = logger;
            _childRepository = childRepository;
        }

        public async Task<List<Child>> GetChildrenByUserInfoIdAsync(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    throw new ArgumentException("User info ID is required to get children.");
                }

                var children = await _childRepository.GetAll(x => x.UserInfoId == userId);
                return children.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting children for User info id {UserId}", userId);
                throw;
            }
        }

        public async Task<Child> CreateChildAsync(Child child)
        {
            try
            {
                if (child == null)
                {
                    return null;
                }

                if (child.UserInfoId == 0)
                {
                    throw new InvalidDataException("User info ID is required to create a child.");
                }

                var newChild = await _childRepository.AddAsync(child);
                if (newChild == null)
                    throw new Exception("Child creation failed.");

                return newChild;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating child with User info id {UserId}", child.UserInfoId);
                throw;
            }
        }

        public async Task<Child> DeleteChildAsync(Child child)
        {
            try
            {
                if(child == null)
                    throw new ArgumentNullException(nameof(child));

                await _childRepository.DeleteAsync(child);
                return child;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting child with ID {ChildId}", child.Id);
                throw;
            }
        }
    }
}
