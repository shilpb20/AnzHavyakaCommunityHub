namespace CommunityHub.Infrastructure.AppMailService
{
    public class RegistrationModel : TemplateModelBase
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
    }
}
