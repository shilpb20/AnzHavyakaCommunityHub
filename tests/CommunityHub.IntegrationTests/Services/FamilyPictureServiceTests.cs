using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommunityHub.IntegrationTests.Services
{
    public class FamilyPictureServiceTests : IAsyncLifetime
    {
        private ServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;
        private FamilyPictureService _familyPictureService;

        public async Task InitializeAsync()
        {
            // Setup in-memory database
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            serviceCollection.AddLogging();

            // Add the repository and service
            serviceCollection.AddRepository<FamilyPicture, ApplicationDbContext>();
            serviceCollection.AddScoped<IFamilyPictureService, FamilyPictureService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = _serviceProvider.GetRequiredService<ILogger<FamilyPictureService>>();
            var repository = _serviceProvider.GetRequiredService<IRepository<FamilyPicture, ApplicationDbContext>>();

            _familyPictureService = new FamilyPictureService(logger, repository);

            await SeedData();
        }

        public Task DisposeAsync()
        {
            _dbContext.Database.EnsureDeleted();
            return Task.CompletedTask;
        }

        private async Task SeedData()
        {
            _dbContext.FamilyPictures.AddRange(new List<FamilyPicture>
            {
                new FamilyPicture { ImageUrl = "/images/family1.jpg", UserInfoId = 1 },
                new FamilyPicture { ImageUrl = "/images/family2.jpg", UserInfoId = 2 }
            });

            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateFamilyPictureAsync_ShouldAddFamilyPicture()
        {
            // Arrange
            var newFamilyPicture = new FamilyPicture
            {
                ImageUrl = "/images/family3.jpg",
                UserInfoId = 3
            };

            // Act
            var result = await _familyPictureService.CreateFamilyPictureAsync(newFamilyPicture);

            // Assert
            result.Should().NotBeNull();
            result.ImageUrl.Should().Be(newFamilyPicture.ImageUrl);

            var familyPictureInDb = await _dbContext.FamilyPictures.FindAsync(result.Id);
            familyPictureInDb.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFamilyPictureAsync_ShouldRemoveFamilyPicture()
        {
            // Arrange
            var familyPictureToDelete = await _dbContext.FamilyPictures.FirstAsync();

            // Act
            await _familyPictureService.DeleteFamilyPictureAsync(familyPictureToDelete);

            // Assert
            var familyPictureInDb = await _dbContext.FamilyPictures.FindAsync(familyPictureToDelete.Id);
            familyPictureInDb.Should().BeNull();
        }

        [Fact]
        public async Task GetAllFamilyPicturesAsync_ShouldReturnAllFamilyPictures()
        {
            // Act
            var familyPictures = await _familyPictureService.GetAllFamilyPicturesAsync();

            // Assert
            familyPictures.Should().NotBeNull();
            familyPictures.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetFamilyPictureByIdAsync_ShouldReturnCorrectFamilyPicture()
        {
            // Arrange
            var familyPicture = await _dbContext.FamilyPictures.FirstAsync();

            // Act
            var result = await _familyPictureService.GetFamilyPictureByIdAsync(familyPicture.Id);

            // Assert
            result.Should().NotBeNull();
            result.ImageUrl.Should().Be(familyPicture.ImageUrl);
        }

        [Fact]
        public async Task GetFamilyPictureByPathAsync_ShouldReturnCorrectFamilyPicture()
        {
            // Arrange
            var path = "/images/family1.jpg";

            // Act
            var result = await _familyPictureService.GetFamilyPictureByPathAsync(path);

            // Assert
            result.Should().NotBeNull();
            result.ImageUrl.Should().Be(path);
        }

        [Fact]
        public async Task UpdateFamilyPictureAsync_ShouldUpdateFamilyPicture()
        {
            // Arrange
            var familyPictureToUpdate = await _dbContext.FamilyPictures.FirstAsync();
            familyPictureToUpdate.ImageUrl = "/images/updated.jpg";

            // Act
            var result = await _familyPictureService.UpdateFamilyPictureAsync(familyPictureToUpdate);

            // Assert
            result.Should().NotBeNull();
            result.ImageUrl.Should().Be("/images/updated.jpg");

            var familyPictureInDb = await _dbContext.FamilyPictures.FindAsync(result.Id);
            familyPictureInDb.ImageUrl.Should().Be("/images/updated.jpg");
        }
    }
}
