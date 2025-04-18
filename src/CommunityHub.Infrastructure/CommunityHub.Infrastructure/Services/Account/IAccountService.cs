﻿using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Login;
using CommunityHub.Infrastructure.Models.Registration;

namespace CommunityHub.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser> CreateAccountAsync(UserInfo userInfo);
        Task<ApplicationUser> DeleteAccountAsync(string applicationUserId);


        Task<string> GenerateTokenAsync(ApplicationUser applicationUser);
        Task<LoginResponse> LoginAsync(string email, string password);

        Task<bool> SendPasswordResetEmailAsync(string email, string appPasswordResetUrl);
        Task<PasswordResetResult> SetNewPasswordAsync(SetPassword model);
    }
}
