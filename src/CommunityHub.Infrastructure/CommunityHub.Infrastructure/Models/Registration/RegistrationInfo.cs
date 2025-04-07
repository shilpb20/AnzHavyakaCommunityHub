using CommunityHub.Core.Dtos;
using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Models.Registration
{
    public class RegistrationInfo
    {
        public UserInfo UserInfo { get; set; }

        public SpouseInfo? SpouseInfo { get; set; }
        public List<Child>? Children { get; set; } = new List<Child>();
    }
}