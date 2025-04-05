namespace CommunityHub.Infrastructure.AppMailService
{
    public class RegistrationApprovalModel : TemplateModelBase
    {
        public string UserName { get; set; }
        public string PasswordSetLink { get; set; }
    }
}
