using AutoMapper;
using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Messages;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models.Registration;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommunityHub.Api.Controllers
{

    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;

        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IRegistrationService _registrationService;
        private readonly IUserInfoValidationService _userInfoValidationService;

        public AdminController(
            ILogger<AdminController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IRegistrationService registrationService,
            IUserService userService,
            IAdminService adminService,
            IUserInfoValidationService userInfoValidationService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;

            _userService = userService;
            _registrationService = registrationService;
            _adminService = adminService;
            _userInfoValidationService = userInfoValidationService;
        }

        [Authorize(Policy = "AdminUser")]
        [HttpPut(ApiRoute.Admin.RejectRequestById)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> RejectRequest(int id, [FromBody] string comment)
        {
            try
            {
                if (id <= 0) return BadRequest(_responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData, ErrorMessage.InvalidId));

                var registrationRequest = await _adminService.RejectRequestAsync(id, comment);
                if (registrationRequest == null) return BadRequest(_responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData,
                    $"Request with ID {id} not found or could not be rejected."));

                var responseObject = _responseFactory.Success(_mapper.Map<RegistrationRequestDto>(registrationRequest));
                return Ok(responseObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rejecting request - {id}", id);
                return StatusCode(500, $"Unexpected error occurred while rejecting request - {id}");
            }
        }

        //[Authorize(Policy = "AdminUser")]
        [HttpPost(ApiRoute.Admin.ApproveRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserInfoDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> ApproveRequest(ApproveRequest approveRequest)
        {
            if (approveRequest == null || approveRequest.Id <= 0)
                return BadRequest(_responseFactory.Failure<UserInfoDto>(ErrorCode.InvalidData, ErrorMessage.InvalidId));

            var request = await _registrationService.GetRequestByIdAsync(approveRequest.Id);
            if (request == null) return NotFound(_responseFactory.Failure<UserInfoDto>(ErrorCode.InvalidData,
                $"Request with ID {approveRequest.Id} not found or could not be approved."));

            var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(request.RegistrationInfo);
            var userContactDto = new UserContact(registrationInfo.UserInfo.Email, registrationInfo.UserInfo.CountryCode, registrationInfo.UserInfo.ContactNumber);

            var spouseContactDto = registrationInfo.SpouseInfo != null ?
                new UserContact(registrationInfo.SpouseInfo.Email, registrationInfo.SpouseInfo.ContactNumber, registrationInfo.SpouseInfo.CountryCode) : null;

            var duplicateUser = await _userInfoValidationService.CheckDuplicateUser(userContactDto, spouseContactDto);
            if (duplicateUser != eDuplicateStatus.NoDuplicate)
            {
                return BadRequest(_responseFactory.Failure<UserInfoDto>(ErrorCode.DuplicateUser, duplicateUser.GetDescription()));
            }

            var userInfo = await _adminService.ApproveRequestAsync(request, approveRequest.SetPasswordUrl);
            var responseObject = _responseFactory.Success(_mapper.Map<UserInfoDto>(userInfo));
            return Ok(responseObject);
        }
    }
}
