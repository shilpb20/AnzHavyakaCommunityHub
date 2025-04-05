using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

namespace CommunityHub.UI.Services.Registration
{
    public interface IRegistrationService
    {
        Task<ApiResponse<RegistrationRequestDto>> SendRegistrationRequestAsync(RegistrationInfoCreateDto registrationData);
    }
}