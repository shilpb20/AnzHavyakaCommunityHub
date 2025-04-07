namespace CommunityHub.Infrastructure.AppMailService.EmailTemplateModels
{
    public class UserEnquiryModel : TemplateModelBase
    {
        public int Id { get; set; }
        public string SubmissionTime { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}