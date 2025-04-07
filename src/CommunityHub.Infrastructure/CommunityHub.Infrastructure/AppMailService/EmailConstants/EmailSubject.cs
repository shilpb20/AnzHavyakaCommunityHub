using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.AppMailService.EmailConstants
{
    public static class EmailSubject
    {
        public const string RegistrationNotification = "New Registration Request";
        public const string RegistrationRequestRejection = "Registration Request Rejected";
        public const string RegistrationRequestApproval = "Registration Request Approved";

        public const string ResetPassword = "Reset Password";

        public const string UserEnquiry = "User Enquiry";
    }
}
