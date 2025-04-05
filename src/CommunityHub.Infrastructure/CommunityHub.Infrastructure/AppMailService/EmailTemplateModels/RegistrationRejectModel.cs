namespace CommunityHub.Infrastructure.AppMailService
{
    public class RegistrationRejectModel : TemplateModelBase
    {
        public string UserName { get; set; }
        public string Location { get; set; }
        public string RegistrationDate { get; set; }
        public string RejectionReason { get; set; }
    }
}
