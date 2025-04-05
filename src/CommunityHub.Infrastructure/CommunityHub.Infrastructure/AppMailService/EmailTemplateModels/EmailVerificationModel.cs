namespace CommunityHub.Infrastructure.AppMailService
{
    public class EmailVerificationModel : TemplateModelBase
    {
        public string UserName { get; set; }
        public string EmailVerificationLink { get; set; }
    }
}
