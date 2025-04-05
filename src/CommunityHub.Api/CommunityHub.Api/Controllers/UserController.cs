using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;
            _userService = userService;
        }

        [Authorize(Policy = "AuthenticatedUser")]
        [HttpGet(ApiRoute.Users.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<UserInfoDto>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse<List<UserInfoDto>>>> GetUsers(
            [FromQuery] string? sortBy,
            [FromQuery] bool ascending = true)
        {
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var users = await _userService.GetUsersAsync(sortBy, ascending);
            if (!users.Any())
            {
                return NoContent();
            }

            var responseObject = _responseFactory.Success(_mapper.Map<List<UserInfoDto>>(users));
            return Ok(responseObject);
        }
    }
}