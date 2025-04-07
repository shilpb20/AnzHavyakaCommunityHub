using AppComponents.Repository.Abstraction;
using Azure.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommunityHub.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<IRegistrationService> _logger;
        private readonly IAppMailService _appMailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<RegistrationRequest, ApplicationDbContext> _repository;

        public RegistrationService(
            ILogger<IRegistrationService> logger,
            IAppMailService appMailService,
            IHttpContextAccessor httpContextAccessor,
            IRepository<RegistrationRequest, ApplicationDbContext> repository)
        {
            _logger = logger;
            _appMailService = appMailService;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public async Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData, string siteUrl)
        {
            try
            {
                if (registrationData == null)
                {
                    _logger.LogDebug("Registration data null. Skipping request creation");
                    return null;
                }

                _logger.LogDebug($"Creating registration request for user - {registrationData?.UserInfo.FullName}.");

                var registrationInfo = JsonConvert.SerializeObject(registrationData);
                var registrationRequest = new RegistrationRequest(registrationInfo);

                var newRequest = await _repository.AddAsync(registrationRequest);

                var model = new RegistrationModel()
                {
                    Id = newRequest.Id,
                    Location = registrationData.UserInfo.Location,
                    UserName = registrationData.UserInfo.FullName,
                    RegistrationDate = newRequest.CreatedAt,
                    Url = siteUrl
                };

                await _appMailService.SendRegistrationNotificationEmailAsync(model);

                return newRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<RegistrationRequest> GetRequestByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid ID: {id}. Cannot retrieve registration request.");
                return null;
            }

            return await _repository.GetAsync(x => x.Id == id);
        }

        public async Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status = eRegistrationStatus.Pending)
        {
            if (status == eRegistrationStatus.All)
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.GetAllAsync(x => x.RegistrationStatus == status.GetEnumMemberValue());
        }

        public async Task<RegistrationRequest> UpdateRequestAsync(RegistrationRequest registrationRequest)
        {
            return await _repository.UpdateAsync(registrationRequest);
        }
    }
}
