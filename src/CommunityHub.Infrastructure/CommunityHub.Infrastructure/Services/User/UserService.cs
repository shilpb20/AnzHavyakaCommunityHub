using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<IUserService> _logger;
        private readonly IRepository<UserInfo, ApplicationDbContext> _userRepository;

        public UserService(ILogger<IUserService> logger,
            ISpouseService spouseService,
            IRepository<UserInfo, ApplicationDbContext> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<UserInfo> GetUserAsyncById(int id)
        {
            return await _userRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<UserInfo>> GetUsersAsync(string? sortBy, bool ascending)
        {
            var orderBy = GetOrderByDictionary(sortBy, ascending);
            var users = await _userRepository.GetAll(orderByClause: orderBy);
            return await users.Include(x => x.SpouseInfo).Include(y => y.Children).ToListAsync();
        }

        private Dictionary<string, bool> GetOrderByDictionary(string? sortBy, bool ascending)
        {
            Dictionary<string, bool> orderBy = new();
            if (sortBy == null)
            {
                orderBy = new Dictionary<string, bool>
                {
                    { "Location", true },
                    { "FullName", true }
                };
            }
            else
            {
                orderBy = new Dictionary<string, bool>()
                {
                    { sortBy, ascending }
                };
            }

            return orderBy;
        }


        public async Task<UserInfo> GetUserInfoByEmail(string email)
        {
            return await _userRepository.GetAsync(x => x.Email == email);
        }

        public async Task<UserInfo> GetUserInfoByContactNumber(string countryCode, string contactNumber)
        {
            return await _userRepository.GetAsync(x => x.CountryCode == countryCode && x.ContactNumber == contactNumber);
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo userInfo)
        {
            try
            {

                var newUser = await _userRepository.AddAsync(userInfo);
                if (newUser == null)
                    throw new Exception("User info creation failed.");

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with ID {UserId}", userInfo.Id);
                throw;
            }
        }
    }
}
