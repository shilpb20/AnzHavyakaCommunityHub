using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Services
{
    public interface ISpouseService
    {
        Task<SpouseInfo> CreateSpouseAsync(SpouseInfo spouseInfo);
        Task<SpouseInfo> GetSpouseInfoByIdAsync(int id);

        Task<SpouseInfo> GetSpouseInfoByEmail(string email);
        Task<SpouseInfo> GetSpouseInfoByPhone(string countryCode, string contactNumber);
        Task<SpouseInfo> GetSpouseInfoByUserInfoIdAsync(int userInfoId);
    }
}
