using AppComponents.Email.Models;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.AppMailService.EmailTemplateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.EmailService
{
    public interface IAppMailService
    {
        Task<EmailStatus> SendRegistrationNotificationEmailAsync(RegistrationModel model);
        Task<EmailStatus> SendRegistrationRejectionEmailAsync(RegistrationRejectModel model);
        Task<EmailStatus> SendRegistrationApprovalEmailAsync(RegistrationApprovalModel model);
        Task<EmailStatus> SendPasswordResetEmailAsync(EmailLink emailLink);
        Task<EmailStatus> SendUserEnquiryMail(UserEnquiryModel model);
    }
}
