using AppComponents.Email.Services;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.AppMailService;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Web;

namespace CommunityHub.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppMailService _appMailService;

        public AccountService(
            ILogger<AccountService> logger,
            IJwtTokenService jwtTokenService,
            IAppMailService appMailService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _jwtTokenService = jwtTokenService;
            _appMailService = appMailService;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateAccountAsync(UserInfo userInfo)
        {
            try
            {
                var applicationUser = new ApplicationUser()
                {
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    NormalizedEmail = userInfo.Email.ToUpper(),
                    NormalizedUserName = userInfo.Email.ToUpper()
                };

                var identityResult = await _userManager.CreateAsync(applicationUser);
                if (!identityResult.Succeeded) return null;

                var roleResult = await _userManager.AddToRoleAsync(applicationUser, Roles.User);
                if (!roleResult.Succeeded) return null;

                return applicationUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {userName}", userInfo.FullName);
                throw;
            }
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
        }


        public async Task<bool> SendPasswordResetEmailAsync(string email, string appPasswordResetUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string encodedToken = HttpUtility.UrlEncode(token);
            string encodedEmail = HttpUtility.UrlEncode(user.Email);

            var resetUrl = $"{appPasswordResetUrl}?email={encodedEmail}&token={encodedToken}";
            var model = new EmailLink
            {
                Email = email,
                Url = resetUrl
            };

            var emailStatus = await _appMailService.SendPasswordResetEmailAsync(model);
            if (!emailStatus.IsSuccess)
            {
                _logger.LogError("Failed to send password reset email to {email}", email);
                return false;
            }

            return true;
        }

        public async Task<PasswordResetResult> SetNewPasswordAsync(SetPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return PasswordResetResult.Failed("User not found.");
            }

            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var validationResult = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

            if (!validationResult.Succeeded)
            {
                return PasswordResetResult.Failed(validationResult.Errors.Select(e => e.Description).ToArray());
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetResult.Succeeded)
            {
                return PasswordResetResult.Failed(resetResult.Errors.Select(e => e.Description).ToArray());
            }

            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return PasswordResetResult.Failed("Failed to confirm email.");
            }

            return PasswordResetResult.SuccessResult();
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var response = new LoginResponse();
            if (user == null)
            {
                response.Message = "Invalid login credentials";
                response.IsAuthenticated = false;
                return response;
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                response.Message = "Invalid login credentials";
                response.IsAuthenticated = false;
                return response;
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            if (string.IsNullOrEmpty(token.Token))
            {
                response.Message = "Failed to generate token";
                response.IsAuthenticated = false;
                return response;
            }

            response.TokenResponse = token;
            response.Message = "Login successful";
            response.IsAuthenticated = true;
            response.UserRoles = await _userManager.GetRolesAsync(user);

            return response;
        }

    }
}
