using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using AppComponents.Email.Models;
using AppComponents.Email.Services;
using AppComponents.TemplateEngine;

using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.AppMailService.EmailConstants;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Settings;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CommunityHub.Infrastructure.EmailService
{
    public class AppMailService : IAppMailService
    {
        private readonly ILogger<AppMailService> _logger;
        private readonly IModelTemplateEngine _templateEngine;
        private readonly IEmailService _emailService;

        private readonly IHostEnvironment _hostEnvironment;

        private readonly EmailAppSettings _settings;

        public AppMailService(
            ILogger<AppMailService> logger,
            IModelTemplateEngine templateEngine,
            IEmailService emailService,
            IHostEnvironment hostEnvironment,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _templateEngine = templateEngine;
            _settings = appSettings.Value.EmailAppSettings;

            _emailService = _settings.EnableEmails ? emailService : null;
        }

        public async Task<EmailStatus> SendRegistrationNotificationEmailAsync(RegistrationModel model)
        {
            model.Title = EmailSubject.RegistrationNotification;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, TemplateNames.RegistrationNotification);
            string content = _templateEngine.Render(template, model);

            var emailRequest = new EmailRequest
            {
                To = new List<string> { _settings.AdminEmail },
                Subject = model.Title,
                HtmlContent = content
            };

            return await _emailService?.SendEmailAsync(emailRequest);
        }

        public async Task<EmailStatus> SendRegistrationApprovalEmailAsync(RegistrationApprovalModel model)
        {
             return await SendEmailToUserAsync(
                 TemplateNames.RegistrationRequestApproval,
                 EmailSubject.RegistrationRequestApproval,
                 model);
        }


        public async Task<EmailStatus> SendRegistrationRejectionEmailAsync(RegistrationRejectModel model)
        {
            return await SendEmailToUserAsync(
                TemplateNames.RegistrationRequestRejection,
                EmailSubject.RegistrationRequestRejection,
                model);
        }

        public async Task<EmailStatus> SendPasswordResetEmailAsync(EmailLink model)
        {
            return await SendEmailToUserAsync(
                TemplateNames.ResetPassword,
                EmailSubject.ResetPassword,
                model);
        }

        private async Task<EmailStatus> SendEmailToUserAsync<T>(string templateName, string subject, T model)
        {
            if(model is TemplateModelBase)
                (model as TemplateModelBase).Title = subject;

            string template = await EmailTemplateGetter.GetTemplateAsync(_settings.EmailTemplateDirectory, templateName);
            string content = _templateEngine.Render<T>(template, model);

            string recipientEmail = _hostEnvironment.IsProduction() ? 
                (model as TemplateModelBase)?.Email : _settings.AdminEmail;
            var emailRequest = new EmailRequest
            {
                To = new List<string> { recipientEmail },
                Subject = model is TemplateModelBase templateModelBase ? templateModelBase.Title : "",

                HtmlContent = content
            };

            return await _emailService?.SendEmailAsync(emailRequest);
        }
    }
}
