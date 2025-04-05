namespace CommunityHub.Core.Dtos
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string? HomeTown { get; set; } = string.Empty;
        public string? HouseName { get; set; }

        public SpouseInfoDto? SpouseInfo { get; set; }
        public List<ChildDto> Children { get; set; } = new List<ChildDto>();
    }
}