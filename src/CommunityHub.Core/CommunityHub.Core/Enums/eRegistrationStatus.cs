using System.Runtime.Serialization;

namespace CommunityHub.Core.Enums
{
    public enum eRegistrationStatus
    {
        [EnumMember(Value = "pending")]
        Pending,

        [EnumMember(Value = "approved")]
        Approved,

        [EnumMember(Value = "rejected")]
        Rejected,

        [EnumMember(Value = "all")]
        All
    }
}
