using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;

namespace CommunityHub.Core.Helpers
{
    public static class RegistrationStatusHelper
    {
        public static readonly string PendingStatus = eRegistrationStatus.Pending.GetEnumMemberValue();
        public static readonly string ApprovedStatus = eRegistrationStatus.Approved.GetEnumMemberValue();
        public static readonly string RejectedStatus = eRegistrationStatus.Rejected.GetEnumMemberValue();
    }
}
