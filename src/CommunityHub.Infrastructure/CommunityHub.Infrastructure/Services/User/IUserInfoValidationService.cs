using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;

namespace CommunityHub.Infrastructure.Services
{
    public interface IUserInfoValidationService
    {
        Task<eDuplicateStatus> CheckDuplicateUser(UserContact userContact, UserContact? spouseContact);
    }
}