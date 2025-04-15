using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models.Login;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ICookieWriterService _cookieService;
        private readonly IResponseFactory _responseFactory;

        private readonly IUserAccountManagementService _accountManagementService;

        public AccountController(
            ILogger<AccountController> logger,
            IMapper mapper,
            IAccountService accountService,
            IUserAccountManagementService accountManagementService,
            ICookieWriterService cookieService,
            IResponseFactory responseFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _cookieService = cookieService;
            _accountService = accountService;
            _accountManagementService = accountManagementService;
            _responseFactory = responseFactory;
        }

        [HttpPost(ApiRoute.Account.Login)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<LoginResponseDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                var response = _responseFactory.Failure<LoginResponse>("Login Error", "Email and password are required.");
                return BadRequest(response);
            }

            var result = await _accountService.LoginAsync(model.Email, model.Password);
            if (result.IsAuthenticated)
            {
                _cookieService.SetCookie(CookiePart.AuthToken, result.TokenResponse.Token, result.TokenResponse.ExpirationTime);
                var response = _responseFactory.Success<LoginResponse>(result);
                return Ok(response);
            }

            var failureResponse = _responseFactory.Failure<LoginResponse>("Login Error", $"Failed to login: {result.Message}");
            return BadRequest(failureResponse);
        }


        [HttpPost(ApiRoute.Account.SendPasswordResetEmail)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> SendPasswordResetEmailAsync(PasswordLink passwordLink)
        {
            var result = await _accountService.SendPasswordResetEmailAsync(passwordLink.Email, passwordLink.Url);
            if (result)
            {
                var response = _responseFactory.Success<bool>(true);
                return Ok(response);
            }

            var failureResponse = _responseFactory.Failure<bool>("Password Reset Error", $"Failed to send password reset email.");
            return BadRequest(failureResponse);
        }


        [HttpPost(ApiRoute.Account.SetPassword)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> SetPasswordAsync([FromBody] SetPassword model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                var response = _responseFactory.Failure<bool>("Password Error", "Passwords do not match");
                return BadRequest(response);
            }

            var result = await _accountService.SetNewPasswordAsync(model);
            if (result.Success)
            {
                var response = _responseFactory.Success<bool>(true);
                return Ok(response);
            }

            var errorMessage = string.Join(", ", result.Errors);
            var failureResponse = _responseFactory.Failure<bool>("Password Error", $"Failed to set password: {errorMessage}");
            return BadRequest(failureResponse);
        }

        [HttpDelete(ApiRoute.Account.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAccountAsync(int id)
        {
            var result = await _accountManagementService.DeleteUserAccountAsync(id);
            if (result != null)
            {
                var response = _responseFactory.Success<bool>(true);
                return Ok(response);
            }

            var failureResponse = _responseFactory.Failure<bool>("Account Deletion Error", $"Failed to delete account.");
            return BadRequest(failureResponse);
        }
    }
}
