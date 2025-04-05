using AppComponents.Repository.EFCore;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.Registration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TestDataHelper = RegistrationTestDataHelper;
using AppComponents.Email.Models;
using AppComponents.Email.Services;
using Moq;
using System.Net.Mail;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace CommunityHub.IntegrationTests.Services
{
    public class RegistrationServiceTests : BaseTests
    {
        #region class-initiazation

        private IRegistrationService _registrationService;
        private Mock<IEmailService> _emailService;
        private readonly string _rejectionReason = "Invalid request";

        public RegistrationServiceTests() : base()
        {
            _registrationService = ServiceProvider.GetRequiredService<IRegistrationService>();
        }

        protected override void AddServices(IServiceCollection services)
        {
            _emailService = new Mock<IEmailService>();
            _emailService
                .Setup(x => x.SendEmailAsync(It.IsAny<EmailRequest>()))
                .ReturnsAsync(new EmailStatus
                {
                    IsSuccess = true,
                    Message = "Email sent successfully"
                });

            services.AddSingleton(_emailService.Object);

            services.AddRepository<RegistrationRequest, ApplicationDbContext>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddLogging();
        }

        #endregion

        #region create-request

        [Fact]
        public async Task CreateRequestAsync_ShouldCreateRequest_WhenRequestIsValid()
        {
            // Arrange
            var registrationData = TestDataHelper.GetTestRegistrationInfo();

            // Act
            var request = await _registrationService.CreateRequestAsync(registrationData);

            // Assert
            request.Should().NotBeNull();

            request.RegistrationStatus.Should().Be(RegistrationStatusHelper.PendingStatus);
            request.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            request.ReviewedAt.Should().BeNull();

            var savedRequest = Context.RegistrationRequests.FirstOrDefault();
            savedRequest.Should().NotBeNull();
            savedRequest.RegistrationInfo.Should().Be(JsonConvert.SerializeObject(registrationData));
        }

        [Fact]
        public async Task CreateRequestAsync_ShouldReturnNull_WhenRegistrationDataIsNull()
        {
            // Act
            var result = await _registrationService.CreateRequestAsync(null);

            // Assert
            result.Should().BeNull();
        }


        #endregion

        //#region update-request

        //[Fact]
        //public async Task ApproveRequestAsync_ShouldApproveRequest_WhenRequestIsPresent()
        //{
        //    // Arrange
        //    var registrationData = TestDataHelper.GetTestRegistrationInfo();
        //    var createdRequest = await _registrationService.CreateRequestAsync(registrationData);

        //    // Act
        //    var updatedRequest = await _registrationService.ApproveRequestAsync(createdRequest.Id);

        //    // Assert
        //    updatedRequest.Should().NotBeNull();
        //    updatedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.ApprovedStatus);

        //    // Check if updated request is saved correctly in DB
        //    var savedRequest = Context.RegistrationRequests.FirstOrDefault(r => r.Id == createdRequest.Id);
        //    savedRequest.Should().NotBeNull();
        //    savedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.ApprovedStatus);
        //}


        //[Fact]
        //public async Task ApproveRequestAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        //{
        //    // Arrange
        //    var registrationData = TestDataHelper.GetTestRegistrationInfo();
        //    var request = await _registrationService.CreateRequestAsync(registrationData);
        //    var nonExistentId = 9999;

        //    // Act
        //    var updatedRequest = await _registrationService.ApproveRequestAsync(nonExistentId);

        //    // Assert
        //    updatedRequest.Should().BeNull();
        //}

        //[Fact]
        //public async Task RejectRequestAsync_ShouldRejectRequest_WhenRequestIsPresent()
        //{
        //    // Arrange
        //    var registrationData = TestDataHelper.GetTestRegistrationInfo();
        //    var createdRequest = await _registrationService.CreateRequestAsync(registrationData);

        //    // Act
        //    var updatedRequest = await _registrationService.RejectRequestAsync(createdRequest.Id, _rejectionReason);

        //    // Assert
        //    updatedRequest.Should().NotBeNull();
        //    updatedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.RejectedStatus);

        //    // Check if updated request is saved correctly in DB
        //    var savedRequest = Context.RegistrationRequests.FirstOrDefault(r => r.Id == createdRequest.Id);
        //    savedRequest.Should().NotBeNull();
        //    savedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.RejectedStatus);
        //}

        //[Fact]
        //public async Task RejectRequestAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        //{
        //    // Arrange
        //    var registrationData = TestDataHelper.GetTestRegistrationInfo();
        //    var request = await _registrationService.CreateRequestAsync(registrationData);
        //    var nonExistentId = 9999;

        //    // Act
        //    var updatedRequest = await _registrationService.RejectRequestAsync(nonExistentId, _rejectionReason);

        //    // Assert
        //    updatedRequest.Should().BeNull();
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData(null)]
        //public async Task RejectRequestAsync_ShouldThrowArgumentException_WhenReasonIsNotProvided(string reason)
        //{
        //    // Arrange
        //    var registrationData = TestDataHelper.GetTestRegistrationInfo();
        //    var createdRequest = await _registrationService.CreateRequestAsync(registrationData);

        //    // Act
        //    var exception = Assert.ThrowsAnyAsync<ArgumentException>(() => _registrationService.RejectRequestAsync(createdRequest.Id, reason));
        //}

        //#endregion

        #region get-request-by-id

        [Fact]
        public async Task GetRequestAsync_ShouldReturnRequest_WhenRequestExists()
        {
            // Arrange
            var registrationData = TestDataHelper.GetTestRegistrationInfo();
            var createdRequest = await _registrationService.CreateRequestAsync(registrationData);

            // Act
            var retrievedRequest = await _registrationService.GetRequestByIdAsync(createdRequest.Id);

            // Assert
            retrievedRequest.Should().NotBeNull();
            retrievedRequest.Id.Should().Be(createdRequest.Id);
            retrievedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.PendingStatus);

            var savedRequest = Context.RegistrationRequests.FirstOrDefault(r => r.Id == createdRequest.Id);
            savedRequest.Should().NotBeNull();
            savedRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.PendingStatus);
            savedRequest.Review.Should().BeNull();
        }

        [Fact]
        public async Task GetRequestAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            //Arrange
            int invalidId = 9999;

            // Act
            var request = await _registrationService.GetRequestByIdAsync(invalidId);

            // Assert
            request.Should().BeNull();
        }


        #endregion

        //#region get-requests-by-status

        //[Fact]
        //public async Task GetRequestsAsync_ShouldReturnAllMatchingRequests_ForMultiplePendingRequests()
        //{
        //    // Arrange
        //    var registrationData1 = TestDataHelper.GetTestRegistrationInfo();
        //    var registrationData2 = TestDataHelper.GetTestRegistrationInfo();
        //    var registrationData3 = TestDataHelper.GetTestRegistrationInfo();

        //    // Create multiple pending requests
        //    await _registrationService.CreateRequestAsync(registrationData1);
        //    await _registrationService.CreateRequestAsync(registrationData2);
        //    await _registrationService.CreateRequestAsync(registrationData3);

        //    // Act
        //    var requests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Pending);

        //    // Assert
        //    requests.Should().HaveCount(3);
        //    requests.All(r => r.RegistrationStatus == RegistrationStatusHelper.PendingStatus).Should().BeTrue();
        //}

        //[Fact]
        //public async Task GetRequestsAsync_ShouldReturnAllMatchingRequests_ForMultipleApprovedRequests()
        //{
        //    // Arrange
        //    var registrationData1 = TestDataHelper.GetTestRegistrationInfo();
        //    var registrationData2 = TestDataHelper.GetTestRegistrationInfo();

        //    // Create requests and update to approved
        //    var request1 = await _registrationService.CreateRequestAsync(registrationData1);
        //    var request2 = await _registrationService.CreateRequestAsync(registrationData2);

        //    await _registrationService.ApproveRequestAsync(request1.Id);
        //    await _registrationService.ApproveRequestAsync(request2.Id);

        //    // Act
        //    var requests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Approved);

        //    // Assert
        //    requests.Should().HaveCount(2);
        //    requests.All(r => r.RegistrationStatus == RegistrationStatusHelper.ApprovedStatus).Should().BeTrue();
        //}

        //[Fact]
        //public async Task GetRequestsAsync_ShouldReturnAllMatchingRequests_ForMultipleRejectedRequests()
        //{
        //    // Arrange
        //    var registrationData1 = TestDataHelper.GetTestRegistrationInfo();
        //    var registrationData2 = TestDataHelper.GetTestRegistrationInfo();

        //    var request1 = await _registrationService.CreateRequestAsync(registrationData1);
        //    var request2 = await _registrationService.CreateRequestAsync(registrationData2);

        //    await _registrationService.RejectRequestAsync(request1.Id, _rejectionReason);
        //    await _registrationService.RejectRequestAsync(request2.Id, _rejectionReason);

        //    // Act
        //    var requests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Rejected);

        //    // Assert
        //    requests.Should().HaveCount(2);
        //    requests.All(r => r.RegistrationStatus == RegistrationStatusHelper.RejectedStatus).Should().BeTrue();
        //}

        //[Fact]
        //public async Task GetRequestsAsync_ShouldReturnAllRequests_WhenStatusIsAll()
        //{
        //    // Arrange
        //    await CreateRequestsWithDifferentStatusEach();

        //    // Act
        //    var requests = await _registrationService.GetRequestsAsync(eRegistrationStatus.All);

        //    // Assert
        //    requests.Should().HaveCount(3);
        //    requests.Count(r => r.RegistrationStatus == RegistrationStatusHelper.ApprovedStatus).Should().Be(1);
        //    requests.Count(r => r.RegistrationStatus == RegistrationStatusHelper.RejectedStatus).Should().Be(1);
        //    requests.Count(r => r.RegistrationStatus == RegistrationStatusHelper.PendingStatus).Should().Be(1);
        //}



        //[Theory]
        //[InlineData(eRegistrationStatus.All)]
        //[InlineData(eRegistrationStatus.Approved)]
        //[InlineData(eRegistrationStatus.Rejected)]
        //[InlineData(eRegistrationStatus.Pending)]
        //public async Task GetRequestsAsync_ShouldReturnEmptyList_WhenNoRequestExists(eRegistrationStatus status)
        //{
        //    // Act
        //    var requests = await _registrationService.GetRequestsAsync(status);

        //    // Assert
        //    requests.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task GetRequestsAsync_ShouldReturnMatchingRequests_OnSingleMatch()
        //{
        //    // Arrange
        //    await CreateRequestsWithDifferentStatusEach();

        //    // Act
        //    var pendingRequests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Pending);
        //    var approvedRequests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Approved);
        //    var rejectedRequests = await _registrationService.GetRequestsAsync(eRegistrationStatus.Rejected);

        //    // Assert
        //    pendingRequests.Should().HaveCount(1);
        //    approvedRequests.Should().HaveCount(1);
        //    rejectedRequests.Should().HaveCount(1);
        //}

        //private async Task CreateRequestsWithDifferentStatusEach()
        //{
        //    var registrationData1 = TestDataHelper.GetTestRegistrationInfo();
        //    var request1 = await _registrationService.CreateRequestAsync(registrationData1);
        //    await _registrationService.ApproveRequestAsync(request1.Id);

        //    var registrationData2 = TestDataHelper.GetTestRegistrationInfo();
        //    var request2 = await _registrationService.CreateRequestAsync(registrationData2);
        //    await _registrationService.RejectRequestAsync(request2.Id, _rejectionReason);

        //    var registrationData3 = TestDataHelper.GetTestRegistrationInfo();
        //    var request3 = await _registrationService.CreateRequestAsync(registrationData3);
        //}


        //#endregion
    }
}
