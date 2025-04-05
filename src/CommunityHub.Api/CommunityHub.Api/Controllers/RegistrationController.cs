using AutoMapper;
using CommunityHub.Api.NamedConstants;
using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Messages;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CommunityHub.Api.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;

        private readonly IRegistrationService _registrationService;
        private readonly IUserService _userService;
        private readonly ISpouseService _spouseService;
        private readonly IUserInfoValidationService _validationService;

        public RegistrationController(
            ILogger<RegistrationController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IRegistrationService registrationService,
            IUserService userService,
            ISpouseService spouseService,
            IUserInfoValidationService validationService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;

            _userService = userService;
            _spouseService = spouseService;
            _registrationService = registrationService;
            _validationService = validationService;
        }


        [HttpPost(ApiRoute.Registration.CreateRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> AddRegistrationRequest([FromBody] RegistrationInfoCreateDto registrationInfoDto)
        {
            ApiResponse<RegistrationRequestDto> responseObject = null;
            var errorResponse = ValidationHelper.ValidateModelState(ModelState, ErrorMessage.InvalidData);
            if (errorResponse != null)
            {
                responseObject = _responseFactory.Failure<RegistrationRequestDto>(errorResponse);
                return BadRequest(responseObject);
            }

            try
            {
                var userContact = new UserContact(registrationInfoDto.UserInfo.Email, registrationInfoDto.UserInfo.CountryCode, registrationInfoDto.UserInfo.ContactNumber);
                var spouseContact = registrationInfoDto.SpouseInfo != null ? new UserContact(registrationInfoDto.SpouseInfo.Email, registrationInfoDto.SpouseInfo.CountryCode, registrationInfoDto.SpouseInfo.ContactNumber) : null;

                eDuplicateStatus duplicateStatus = await _validationService.CheckDuplicateUser(userContact, spouseContact);
                if (duplicateStatus != eDuplicateStatus.NoDuplicate)
                {
                    responseObject = _responseFactory.Failure<RegistrationRequestDto>(ErrorCode.DuplicateUser, duplicateStatus.GetDescription());
                    return BadRequest(responseObject);
                }

                var registrationInfo = _mapper.Map<RegistrationInfo>(registrationInfoDto);
               
                var registrationRequest = await _registrationService.CreateRequestAsync(registrationInfo, registrationInfoDto.Url);
                var registrationRequestDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);

                responseObject = _responseFactory.Success<RegistrationRequestDto>(registrationRequestDto);
                return CreatedAtRoute(RouteNames.Registration.GetRequest, new { id = registrationRequestDto.Id }, responseObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogError.Registration.AddRequestProcessingError);
                _logger.LogError(String.Format(LogError.Registration.AdRequestErrorInfo, ex.Message));

                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.Common.RequestProcessingError);
            }
        }

        [Authorize(Policy = "AdminUser")]
        [HttpGet(ApiRoute.Registration.GetRequests)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationRequestDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<List<RegistrationRequestDto>>>> GetRequests([FromQuery] string status = "pending")
        {
            ApiResponse<List<RegistrationRequestDto>> responseObject = null;
            if (Enum.TryParse<eRegistrationStatus>(status, true, out var registrationStatus)
                && Enum.IsDefined(typeof(eRegistrationStatus), registrationStatus))
            {
                var requests = await _registrationService.GetRequestsAsync(registrationStatus);
                if (!requests.Any()) { return NoContent(); }

                var requestDtos = _mapper.Map<List<RegistrationRequestDto>>(requests);
                responseObject = _responseFactory.Success<List<RegistrationRequestDto>>(_mapper.Map<List<RegistrationRequestDto>>(requestDtos));
                return Ok(responseObject);
            }
            else
            {
                responseObject = _responseFactory.Failure<List<RegistrationRequestDto>>(ErrorCode.InvalidData,
                    $"Invalid registration status value: {status}. " +
                    $"Valid values are {string.Join(", ", Enum.GetNames(typeof(eRegistrationStatus)))}");

                return BadRequest(responseObject);
            }
        }

        [Authorize(Policy = "AdminUser")]
        [HttpGet(ApiRoute.Registration.GetRequestById, Name = RouteNames.Registration.GetRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> GetRegistrationRequest(int id)
        {
            ApiResponse<RegistrationRequestDto> responseObject = null;
            if (id <= 0)
            {
                responseObject = _responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData, ErrorMessage.InvalidId);
                return BadRequest(responseObject);
            }

            var result = await _registrationService.GetRequestByIdAsync(id);
            if (result == null)
            {
                responseObject = _responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData, $"Request with Id - {id} is not found.");
                return NotFound(responseObject);
            }

            var resultDto = _mapper.Map<RegistrationRequestDto>(result);
            responseObject = _responseFactory.Success(resultDto);
            return Ok(responseObject);
        }
    }
}
