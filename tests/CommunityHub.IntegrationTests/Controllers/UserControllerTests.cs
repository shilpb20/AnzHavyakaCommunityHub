using CommunityHub.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq; // For mocking dependencies
using Microsoft.AspNetCore.Mvc;
using CommunityHub.Api.Controllers;
using CommunityHub.Infrastructure.Services.User;
using CommunityHub.Core.Dtos;
using CommunityHub.Tests.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityHub.Infrastructure.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CommunityHub.IntegrationTests;

namespace CommunityHub.Tests.Controllers
{
    public class UserControllerTests : BaseTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UserController>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly UserController _controller;

        public UserControllerTests() : base()
        {
            _logger = new Mock<ILogger<UserController>>();
            _mapper = new Mock<IMapper>();
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_logger.Object, _mapper.Object, _mockUserService.Object);
        }

        private async Task SeedUsersAsync(List<UserInfo> users)
        {
            _mockUserService.Setup(service => service.GetUsersAsync())
                            .ReturnsAsync(users);
        }

        private async Task<ActionResult<List<UserInfoDto>>> GetUsersAsync(string sortBy = null, bool ascending = true)
        {
            return await _controller.GetUsers(sortBy, ascending);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnNoContent_WhenNoUsersExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUsersAsync())
                .ReturnsAsync(new List<UserInfo>());

            // Act
            var result = await GetUsersAsync();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSortedByName_AZ_WhenSortByNameIsPassedAndAscendingIsTrue()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync("FullName", true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal("Alice", returnedUsers[0].FullName);
            Assert.Equal("Bob", returnedUsers[1].FullName);
            Assert.Equal("Charlie", returnedUsers[2].FullName);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSortedByName_ZA_WhenSortByNameIsPassedAndAscendingIsFalse()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync("FullName", false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal("Charlie", returnedUsers[0].FullName);
            Assert.Equal("Bob", returnedUsers[1].FullName);
            Assert.Equal("Alice", returnedUsers[2].FullName);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSortedByLocation_AZ_WhenSortByLocationIsPassedAndAscendingIsTrue()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync("Location", true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal("Alice", returnedUsers[0].FullName);
            Assert.Equal("Bob", returnedUsers[1].FullName);
            Assert.Equal("Charlie", returnedUsers[2].FullName);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSortedByLocation_ZA_WhenSortByLocationIsPassedAndAscendingIsFalse()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync("Location", false);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal("Charlie", returnedUsers[0].FullName);
            Assert.Equal("Bob", returnedUsers[1].FullName);
            Assert.Equal("Alice", returnedUsers[2].FullName);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSortedByDefault_WhenNoSortByIsPassed()
        {
            // Arrange
            var users = UserTestsDataHelper.GetUsers();
            await SeedUsersAsync(users);

            // Act
            var result = await GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserInfoDto>>(okResult.Value);
            Assert.Equal("Alice", returnedUsers[0].FullName);
            Assert.Equal("Bob", returnedUsers[1].FullName);
            Assert.Equal("Charlie", returnedUsers[2].FullName);
        }

        protected override void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>(); 
        }
    }
}
