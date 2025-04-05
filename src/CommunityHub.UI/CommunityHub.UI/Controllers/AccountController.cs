using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.UI.Services.Account;
using CommunityHub.UI.Services.Registration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Contracts;

namespace CommunityHub.UI.Controllers
{
    public class AccountController : Controller
    {
        #region fields and initializers

        private readonly ILogger<AccountController> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IAccountService _accountService;
        private readonly ICookieStorage _cookiStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IOptions<AppSettings> _options;

        public AccountController(
            ILogger<AccountController> logger,
            IOptions<AppSettings> options,
            IRegistrationService registrationService,
            IAccountService accountService,
            ICookieStorage cookieStorage)
        {
            _logger = logger;
            _options = options;

            _cookiStorage = cookieStorage;
            _registrationService = registrationService;
            _accountService = accountService;
        }

        #endregion

        #region register-account

        [HttpGet(UiRoute.Account.Register)]
        public IActionResult Register()
        {
            var registrationData = new RegistrationInfoCreateDto()
            {
                UserInfo = new UserInfoCreateDto(),
                SpouseInfo = null,
                Children = new List<ChildCreateDto>()
            };

            return View(registrationData);
        }

        
        [HttpPost(UiRoute.Account.Register)]
        public async Task<IActionResult> Add([FromForm] RegistrationInfoCreateDto registrationData)
        {
            ErrorResponse? errorResponse = ValidationHelper.ValidateModelState(ModelState, "");
            if (errorResponse != null)
            {
                ViewBag.ErrorMessages = errorResponse.ErrorMessage;
                return View("register", registrationData);
            }

            registrationData.Url = _options.Value.SiteUrl;
            var result = await _registrationService.SendRegistrationRequestAsync(registrationData);
            if (!result.Success)
            {
                ModelState.AddModelError(result.ErrorCode, result.ErrorMessage);
                return View("register", registrationData);
            }

            TempData["SuccessMessage"] = "Registration request has been sent for admin approval!";
            return RedirectToAction("login");
        }

        #endregion

        #region set-password

        [HttpGet(UiRoute.Account.ResetPassword)]
        [HttpGet(UiRoute.Account.SetPassword)]
        public IActionResult SetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Error", "Home");
            }
          
            return View();
        }

        [HttpPost(UiRoute.Account.ResetPassword)]
        [HttpPost(UiRoute.Account.SetPassword)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            var result = await _accountService.SetNewPasswordAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError("Password", result.ErrorMessage);
                return View(model);
            }

            return RedirectToAction(nameof(SetPasswordConfirmation));
        }

        public IActionResult SetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region login

        [HttpGet(UiRoute.Account.Login)]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost(UiRoute.Account.Login)]
        public async Task<IActionResult> Login([FromForm] Login model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.ErrorMessage = "Email and password are required.";
                return View(model);
            }

            var result = await _accountService.LoginAsync(model);
            var loginResponse = result.Data;
            if (!result.Success)
            {
                ViewBag.ErrorMessage = "Invalid login credentials";
                return View(model);
            }

            SyncAuthenticationWithApi();
            return RedirectToAction("Index", "Home");
        }
       
        private async Task SyncAuthenticationWithApi()
        {
            var authToken = _cookiStorage.GetCookie(CookiePart.AuthToken);
            if (string.IsNullOrEmpty(authToken))
            {
                return;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(authToken) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    _logger.LogError("Invalid JWT token format.");
                    return;
                }

                var expClaim = jsonToken?.Payload.Exp;
                if (expClaim.HasValue && DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds(expClaim.Value).UtcDateTime)
                {
                    _logger.LogWarning("JWT token has expired.");
                    return;
                }

                var claims = new List<Claim>();
                var usernameClaim = jsonToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (!string.IsNullOrEmpty(usernameClaim))
                {
                    claims.Add(new Claim(ClaimTypes.Name, usernameClaim));
                }

                var userRolesClaim = jsonToken?.Claims.Where(c => c.Type == ClaimTypes.Role)?.ToList();
                foreach (var userRole in userRolesClaim)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Value));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error syncing authentication with API: {ex.Message}");
            }
        }


        #endregion

        #region log-out

        [HttpPost(UiRoute.Account.Logout)]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(CookiePart.AuthToken);
            _cookiStorage.ClearCookies();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            CacheControlHelper.SetNoCacheHeaders(Response);
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        #endregion

        #region forgot-password

        [HttpGet(UiRoute.Account.ForgotPassword)]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost(UiRoute.Account.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your registered email.");
                return View();
            }

            var apiResponse = await _accountService.SendPasswordResetEmailAsync(email);
            TempData["SuccessMessage"] = "If this email is registered, you will receive a password reset link.";
            return RedirectToAction("ForgotPassword");
        }

        #endregion
    }
}
