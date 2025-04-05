namespace CommunityHub.Infrastructure.Settings
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public TransactionSettings TransactionSettings { get; set; }
        public EmailAppSettings EmailAppSettings { get; set; }
    }
}
