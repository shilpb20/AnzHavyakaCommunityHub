namespace CommunityHub.Infrastructure.Settings
{
    public class EmailAppSettings
    {
        public bool EnableEmails { get; set; }
        public string AdminEmail { get; set; }
        public string EmailTemplateDirectory { get; set; }
    }
}
