using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
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
                    throw new Exception("User info ID is required to create a child.");
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
    }
}
