namespace CommunityHub.Core.Dtos
{
    public class RegistrationInfoCreateDto
    {
        public UserInfoCreateDto UserInfo { get; set; }
        public SpouseInfoCreateDto? SpouseInfo { get; set; }
        public List<ChildCreateDto>? Children { get; set; } = new List<ChildCreateDto>();

        public string? Url { get; set; }
    }
}