namespace CommunityHub.Core.Dtos
{
    public class RegistrationRequestDto
    {
        public int Id { get; set; }

        public RegistrationInfoDto RegistrationInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RegistrationStatus { get; set; } = string.Empty;
        public DateTime? ReviewedAt { get; set; }
        public string? Review { get; set; }
    }
}