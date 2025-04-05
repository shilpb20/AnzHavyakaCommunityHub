
namespace CommunityHub.Core.Dtos
{
    public class RegistrationInfoDto
    {
        public UserInfoDto UserInfo { get; set; }

        public SpouseInfoDto? SpouseInfo { get; set; }
        public List<ChildDto>? Children { get; set; } = new List<ChildDto>();
    }
}