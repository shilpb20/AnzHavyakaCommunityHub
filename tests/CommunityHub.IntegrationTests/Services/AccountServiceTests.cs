using CommunityHub.Core.Constants;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;


namespace CommunityHub.IntegrationTests.Services
{
    public class AccountServiceTests
    {
        private ServiceProvider _serviceProvider;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private AccountService _accountService;

        public AccountServiceTests()
        {
            // Setup in-memory UserManager mock
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            // Register services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped(_ => _userManagerMock.Object);
            serviceCollection.AddScoped<IAccountService, AccountService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _accountService = _serviceProvider.GetRequiredService<IAccountService>() as AccountService;
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUserAndAddToRole()
        {
            // Arrange
            var newUser = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            // Mock behavior of UserManager
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), Roles.User))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.CreateUserAsync(newUser);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();

            _userManagerMock.Verify(m => m.CreateAsync(It.Is<ApplicationUser>(u => u == newUser)), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.Is<ApplicationUser>(u => u == newUser), Roles.User), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnFailureIfUserCreationFails()
        {
            // Arrange
            var newUser = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            // Mock behavior of UserManager to return failure
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed." }));

            // Act
            var result = await _accountService.CreateUserAsync(newUser);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.Description == "User creation failed.");

            _userManagerMock.Verify(m => m.CreateAsync(It.Is<ApplicationUser>(u => u == newUser)), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnFailureIfAddingToRoleFails()
        {
            // Arrange
            var newUser = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            // Mock behavior of UserManager
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), Roles.User))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add to role." }));

            // Act
            var result = await _accountService.CreateUserAsync(newUser);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.Description == "Failed to add to role.");

            _userManagerMock.Verify(m => m.CreateAsync(It.Is<ApplicationUser>(u => u == newUser)), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.Is<ApplicationUser>(u => u == newUser), Roles.User), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var userToDelete = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            _userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.DeleteAccountAsync(userToDelete);

            // Assert
            result.Succeeded.Should().BeTrue();
            _userManagerMock.Verify(m => m.DeleteAsync(It.Is<ApplicationUser>(u => u == userToDelete)), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFailureIfDeleteFails()
        {
            // Arrange
            var userToDelete = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            _userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Delete failed." }));

            // Act
            var result = await _accountService.DeleteAccountAsync(userToDelete);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.Description == "Delete failed.");
            _userManagerMock.Verify(m => m.DeleteAsync(It.Is<ApplicationUser>(u => u == userToDelete)), Times.Once);
        }
    }
}
