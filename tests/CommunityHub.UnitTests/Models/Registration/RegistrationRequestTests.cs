using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Messages;
using CommunityHub.Infrastructure.Models;
using FluentAssertions;

namespace CommunityHub.UnitTests.Models
{
    public class RegistrationRequestTests
    {
        public readonly string _registrationData = "test data";
        public readonly string _comment = "invalid request";

        [Fact]
        public void ObjectCreation_ShouldCreateObject_OnProperInstantiation()
        {
            //Act
            var registrationRequest = new RegistrationRequest(_registrationData);

            // Assert
            registrationRequest.RegistrationStatus.Should().Be(eRegistrationStatus.Pending.GetEnumMemberValue());
            registrationRequest.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            registrationRequest.ReviewedAt.Should().BeNull();
            registrationRequest.Review.Should().BeNullOrEmpty();
            registrationRequest.RegistrationInfo.Should().Be(_registrationData); ;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ObjectCreation_ShouldThrowArgumentException_ForInvalidParameters(string registrationInfo)
        {
            //Act
            var exception = Assert.Throws<ArgumentException>(() => new RegistrationRequest(registrationInfo));
        }


        [Fact]
        public void ApproveRequest_ShouldSucceed_WhenCurrentStatusIsPending()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest(_registrationData);

            // Act and Assert
            VerifyValidRegistrationStatusTransition(registrationRequest, RegistrationStatusHelper.ApprovedStatus);
        }

        [Fact]
        public void RejectRequest_ShouldSucceed_WhenCurrentStatusIsPending()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest(_registrationData);

            // Act and Assert
            VerifyValidRegistrationStatusTransition(registrationRequest, RegistrationStatusHelper.RejectedStatus);
        }

        [Fact]
        public void InvalidStateTransitions_ShouldThrowInvalidOperationException_01()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest(_registrationData);
            registrationRequest.Approve();

            ApplyInvalidStateTransitions(registrationRequest, RegistrationStatusHelper.ApprovedStatus);
        }

        [Fact]
        public void InvalidStateTransitions_ShouldThrowInvalidOperationException_02()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest(_registrationData);
            registrationRequest.Reject(_comment);

            ApplyInvalidStateTransitions(registrationRequest, RegistrationStatusHelper.RejectedStatus);
        }

        private void ApplyInvalidStateTransitions(RegistrationRequest request, string status)
        {
            //Act
            var expectedErrorMessage1 = string.Format(ErrorMessage.InvalidStateTransition, status);
            var exception1 = Assert.Throws<InvalidOperationException>(() => request.Approve());
            Assert.Contains(expectedErrorMessage1, exception1.Message.ToString());

            //Act
            var expectedErrorMessage2 = string.Format(ErrorMessage.InvalidStateTransition, status);
            var exception2 = Assert.Throws<InvalidOperationException>(() => request.Reject(_comment));
            Assert.Contains(expectedErrorMessage2, exception2.Message.ToString());
        }

        private void VerifyValidRegistrationStatusTransition(RegistrationRequest registrationRequest, string expectedStatus)
        {
            //Arrange
            var initialRegistrationInfo = registrationRequest.RegistrationInfo;
            var initialRegistrationStatus = registrationRequest.RegistrationStatus;
            var initialReview = registrationRequest.Review;
            var initialCreatedAt = registrationRequest.CreatedAt;
            var initialReviewedAt = registrationRequest.ReviewedAt;

            if (expectedStatus == RegistrationStatusHelper.RejectedStatus)
            {
                //Act
                registrationRequest.Reject(_comment);

                //Assert
                registrationRequest.Review.Should().Be(_comment);
                registrationRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.RejectedStatus);
            }
            else if (expectedStatus == RegistrationStatusHelper.ApprovedStatus)
            {
                //Act
                registrationRequest.Approve();

                //Assert
                registrationRequest.Review.Should().Be(initialReview);
                registrationRequest.RegistrationStatus.Should().Be(RegistrationStatusHelper.ApprovedStatus);
            }

            registrationRequest.ReviewedAt.Should().NotBe(initialReviewedAt);
            registrationRequest.ReviewedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(100));

            registrationRequest.RegistrationInfo.Should().Be(initialRegistrationInfo);
            registrationRequest.CreatedAt.Should().Be(initialCreatedAt);
        }
    }
}
