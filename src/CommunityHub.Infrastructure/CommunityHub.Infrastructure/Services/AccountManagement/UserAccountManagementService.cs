using AppComponents.Email.Services;
using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services
{
    public class UserAccountManagementService : IUserAccountManagementService
    {
        private readonly ILogger<IUserAccountManagementService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ISpouseService _spouseService;
        private readonly IChildService _childService;

        public UserAccountManagementService(
            ILogger<IUserAccountManagementService> logger,
            ITransactionManager transactionManager,
            IAccountService accountService,
            IUserService userService,
            ISpouseService spouseService,
            IChildService childService)
        {
            _logger = logger;
            _transactionManager = transactionManager;
            _accountService = accountService;

            _userService = userService;
            _spouseService = spouseService;
            _childService = childService;
        }

        public async Task<UserInfo> CreateUserAccountAsync(RegistrationInfo registrationInfo)
        {
            var userInfo = registrationInfo.UserInfo;
            var applicationUser = await _accountService.CreateAccountAsync(registrationInfo.UserInfo);
            if (applicationUser == null)
            {
                await _transactionManager.RollbackTransactionAsync();
                return null;
            };

            userInfo.ApplicationUserId = applicationUser.Id;
            var newUser = await _userService.CreateUserAsync(userInfo);

            var spouseInfo = registrationInfo.SpouseInfo;
            if (spouseInfo != null)
            {
                spouseInfo.UserInfoId = newUser.Id;
                await _spouseService.CreateSpouseAsync(spouseInfo);
            }

            var children = registrationInfo.Children;
            if (children.Any())
            {
                foreach (var child in children)
                {
                    child.UserInfoId = newUser.Id;
                    await _childService.CreateChildAsync(child);
                }
            }

            return userInfo;
        }

        public async Task<bool> DeleteUserAccountAsync(int userId)
        {
            await _transactionManager.BeginTransactionAsync();
            try
            {
                var user = await _userService.GetUserAsyncById(userId);
                if (user == null)
                {
                    return false;
                }

                var spouse = await _spouseService.GetSpouseInfoByUserInfoIdAsync(userId);
                if (spouse != null)
                {
                    await _spouseService.DeleteSpouseAsync(spouse);
                }

                var children = await _childService.GetChildrenByUserInfoIdAsync(userId);
                foreach (var child in children)
                {
                    await _childService.DeleteChildAsync(child);
                }

                await _userService.DeleteUserAsync(user);
                await _accountService.DeleteAccountAsync(user.ApplicationUserId);

                await _transactionManager.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting user account with ID {UserId}", userId);
                throw;
            }
        }
    }
}
