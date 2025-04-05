using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Logging;

namespace CommunityHub.Infrastructure.Services
{
    public class UserInfoValidatorService : IUserInfoValidationService
    {
        private readonly ILogger<IUserInfoValidationService> _logger;
        private readonly IUserService _userService;
        private readonly ISpouseService _spouseService;

        public UserInfoValidatorService(ILogger<IUserInfoValidationService> logger,
            IUserService userService,
            ISpouseService spouseService)
        {
            _logger = logger;
            _userService = userService;
            _spouseService = spouseService;
        }

        public async Task<eDuplicateStatus> CheckDuplicateUser(UserContact userContact, UserContact? spouseContact)
        {
            var duplicateUser = await _userService.GetUserInfoByEmail(userContact.Email);
            if (duplicateUser != null)
            {
                return eDuplicateStatus.DuplicateUserEmail;
            }

            duplicateUser = await _userService.GetUserInfoByContactNumber(userContact.CountryCode, userContact.ContactNumber);
            if (duplicateUser != null)
            {
                return eDuplicateStatus.DuplicateUserContact;
            }

            if (spouseContact != null)
            {
                var duplicateSpouse = await _spouseService.GetSpouseInfoByEmail(spouseContact.Email);
                if (duplicateSpouse != null)
                {
                    return eDuplicateStatus.DuplicateSpouseEmail;
                }

                duplicateSpouse = await _spouseService.GetSpouseInfoByPhone(spouseContact.CountryCode, spouseContact.ContactNumber);
                if (duplicateSpouse != null)
                {
                    return eDuplicateStatus.DuplicateSpouseContact;
                }
            }

            return eDuplicateStatus.NoDuplicate;
        }
    }
}
