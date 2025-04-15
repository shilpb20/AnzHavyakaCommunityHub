using AppComponents.Email.Models;
using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web;

namespace CommunityHub.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<IAdminService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRegistrationService _registrationService;
        private readonly IAccountService _accountService;
        private readonly IUserAccountManagementService _accountManagementService;
        private readonly IAppMailService _mailService;

        public AdminService(
            ILogger<IAdminService> logger,
            ITransactionManager transactionManager,
            IRegistrationService registrationService,
            IAccountService accountService,
            IUserAccountManagementService accountManagementService,
            IAppMailService mailService)
        {
            _logger = logger;
            _transactionManager = transactionManager;
            _registrationService = registrationService;
            _accountService = accountService;
            _accountManagementService = accountManagementService;
            _mailService = mailService;
        }

        public async Task<UserInfo> ApproveRequestAsync(
            RegistrationRequest registrationRequest,
            string appSetPasswordUrl)
        {
            await _transactionManager.BeginTransactionAsync();
            
            try
            {
                var matchingRequest = await _registrationService.GetRequestByIdAsync(registrationRequest.Id);
                if (matchingRequest == null) return null;

                var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(matchingRequest.RegistrationInfo);

                var userInfo = await _accountManagementService.CreateUserAccountAsync(registrationInfo);
                if(userInfo == null)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    return null;
                }

                matchingRequest.Approve();
                await _registrationService.UpdateRequestAsync(matchingRequest);

                await _transactionManager.CommitTransactionAsync();

                var emailStatus = await SendConfirmationEmail(
                    userInfo.ApplicationUser,
                    registrationInfo.UserInfo.FullName,
                    appSetPasswordUrl);
                
                return userInfo;
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                _logger.LogError(ex, "Error approving request {Id}", registrationRequest.Id);
                throw;
            }
        }

        private async Task<EmailStatus> SendConfirmationEmail(
            ApplicationUser applicationUser,
            string userName,
            string appSetPasswordUrl)
        {
            string token = await _accountService.GenerateTokenAsync(applicationUser);
            string encodedEmail = HttpUtility.UrlEncode(applicationUser.Email);
            string encodedToken = HttpUtility.UrlEncode(token);
            var model = new RegistrationApprovalModel()
            {
                UserName = userName,
                Email = applicationUser.Email,
                PasswordSetLink = $"{appSetPasswordUrl}?email={encodedEmail}&token={encodedToken}",
            };

            return await _mailService.SendRegistrationApprovalEmailAsync(model);
        }

        public async Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment)
        {
            var registrationRequest = await _registrationService.GetRequestByIdAsync(id);
            if (registrationRequest == null) return null;

            registrationRequest.Reject(reviewComment);
            var result = await _registrationService.UpdateRequestAsync(registrationRequest);

            var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(registrationRequest.RegistrationInfo);
            var registrationRejectModel = new RegistrationRejectModel()
            {
                Email = registrationInfo.UserInfo.Email,
                UserName = registrationInfo.UserInfo.FullName,
                RejectionReason = reviewComment,
                Location = registrationInfo.UserInfo.Location,
                RegistrationDate = registrationRequest.CreatedAt.ToString("dd MMM yyyy")
            };

            var emailStatus = await _mailService.SendRegistrationRejectionEmailAsync(registrationRejectModel);
            return result;
        }
    }
}