using AppComponents.Email.Models;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.AppMailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.EmailService
{
    public interface IAppMailService
    {
        public Task<EmailStatus> SendRegistrationNotificationEmailAsync(RegistrationModel model);
        public Task<EmailStatus> SendRegistrationRejectionEmailAsync(RegistrationRejectModel model);
        public Task<EmailStatus> SendRegistrationApprovalEmailAsync(RegistrationApprovalModel model);
        public Task<EmailStatus> SendPasswordResetEmailAsync(EmailLink emailLink);
    }
}
