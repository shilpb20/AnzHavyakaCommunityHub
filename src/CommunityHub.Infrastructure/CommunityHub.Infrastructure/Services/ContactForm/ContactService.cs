using AppComponents.Repository.Abstraction;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models.Registration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityHub.Infrastructure.AppMailService;
using Newtonsoft.Json;
using CommunityHub.Infrastructure.AppMailService.EmailTemplateModels;

namespace CommunityHub.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly ILogger<IContactService> _logger;
        private readonly IAppMailService _appMailService;
        private readonly IRepository<ContactForm, ApplicationDbContext> _repository;

        public ContactService(
            ILogger<IContactService> logger,
            IAppMailService appMailService,
            IRepository<ContactForm, ApplicationDbContext> repository)
        {
            _logger = logger;
            _appMailService = appMailService;
            _repository = repository;
        }

        public async Task<ContactForm> CreateAddUserEnquiryAsync(ContactForm enquiryData)
        {
            try
            {
                if (enquiryData == null)
                {
                    _logger.LogDebug("Contact Form data null. Request not created.");
                    return null;
                }

                var newEnquiry = await _repository.AddAsync(enquiryData);
                var model = new UserEnquiryModel()
                {
                    Id = newEnquiry.Id,
                    SubmissionTime = newEnquiry.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                    Name = enquiryData.Name,
                    Email = enquiryData.Email,
                    Location = enquiryData.Location,
                    Subject = enquiryData.Subject,
                    Message = enquiryData.Message
                };

                var emailStatus = await _appMailService.SendUserEnquiryMail(model);
                return newEnquiry;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
                throw;
            }
        }
    }
}
