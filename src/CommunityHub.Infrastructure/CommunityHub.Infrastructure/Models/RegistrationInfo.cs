using CommunityHub.Core.Dtos;

namespace CommunityHub.Infrastructure.Models
{
    public class RegistrationInfo
    {
        public UserInfo UserInfo { get; set; }

        public SpouseInfo? SpouseInfo { get; set; }
        public List<Child>? Children { get; set; } = new List<Child>();
    }
}