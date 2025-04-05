using AppComponents.Email.Models;
using AppComponents.Email.Services;
using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.Registration;
using CommunityHub.Infrastructure.Services.User;
using CommunityHub.IntegrationTests;
using CommunityHub.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CommunityHub.IntegrationTests.Services
{
    public class UserServiceTests : BaseTests
    {
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userService = ServiceProvider.GetRequiredService<IUserService>();
        }

        protected override void AddServices(IServiceCollection services)
        {
            services.AddRepository<UserInfo, ApplicationDbContext>();
            services.AddRepository<SpouseInfo, ApplicationDbContext>();
            services.AddRepository<Child, ApplicationDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddLogging();
        }

        [Fact]
        public async Task GetUserAsyncById_ShouldReturnUserWithDetails_WhenUserExists()
        {
            // Arrange
            var users = await SeedDataAsync();

            // Act
            var result = await _userService.GetUserAsyncById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.FullName);
            Assert.NotNull(result.SpouseInfo);
            Assert.Equal("Jane Doe", result.SpouseInfo.FullName);
            Assert.Equal(2, result.Children.Count);
            Assert.Contains(result.Children, c => c.Name == "Child One");
            Assert.Contains(result.Children, c => c.Name == "Child Two");
        }

        [Fact]
        public async Task GetUserAsyncById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var result = await _userService.GetUserAsyncById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers_WhenUsersExist()
        {
            // Arrange
            var users = await SeedDataAsync();

            // Act
            var result = await _userService.GetUsersAsync(null, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
            Assert.All(users, user =>
                Assert.Contains(result, r => r.Id == user.Id && r.FullName == user.FullName));
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Act
            var result = await _userService.GetUsersAsync(null, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        private async Task<List<UserInfo>> SeedDataAsync()
        {
            var users = UserTestsDataHelper.GetUsers();
            Context.UserInfo.AddRange(users);
            await Context.SaveChangesAsync();
            return users;
        }
    }
}
