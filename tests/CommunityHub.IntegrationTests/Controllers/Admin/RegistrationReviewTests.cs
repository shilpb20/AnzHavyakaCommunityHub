using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
using CommunityHub.Infrastructure.Models;
using CommunityHub.IntegrationTests;
using CommunityHub.IntegrationTests.TestData;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CommunityHub.Integrations.Controllers.Admin
{
    public class RegistrationReviewTests : BaseTestEnv, IAsyncLifetime
    {
        private readonly string _registrationRequest = ApiRoute.Registration.Request;
        private readonly string _rejectRequest = ApiRoute.Admin.RejectRequestById;
        private readonly string _approveRequest = ApiRoute.Admin.ApproveRequestById;

        private List<RegistrationRequestDto> _registrationRequests;

        public RegistrationReviewTests(ApplicationStartup application) : base(application)
        {
        }

        public async Task InitializeAsync()
        {
            _registrationRequests = await SeedRegistrationRequests();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }


        public async Task<List<RegistrationRequestDto>> SeedRegistrationRequests()
        {
            var createdRequests = new List<RegistrationRequestDto>();

            var registrationRequests = RegistrationDataList.GetRequests();
            foreach (var registrationData in registrationRequests)
            {
                var result = await HttpSendRequestHelper
                    .SendPostRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(_httpClient, ApiRoute.Registration.Request, registrationData);

                Assert.Equivalent(registrationData, result.RegistrationInfo);

                createdRequests.Add(result);
            }

            return createdRequests;
        }

        #region get- requests

        [Theory]
        [InlineData("")]
        [InlineData("all")]
        [InlineData("pending")]
        public async Task GetRequests_ShouldReturnSuccessAndAllPendingRequests_WhenValidRequestIsSent(string registrationStatus)
        {
            //Arrange
            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = _registrationRequest,
                Query = $"registrationStatus={registrationStatus}"
            };


            //Act
            var result = await HttpSendRequestHelper.SendGetRequestAsync<List<RegistrationRequestDto>>(_httpClient, uriBuilder.ToString());

            //Assert
            Assert.NotNull(result);
        }


        [Theory]
        [InlineData("approved")]
        [InlineData("rejected")]
        public async Task GetRequests_ShouldReturnNoContent_WhenMatchingDataIsNotFound(string registrationStatus)
        {
            //Arrange
            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Registration.Status, registrationStatus }
            };
            var uriBuilder = HttpHelper.BuildUri(_httpClient.BaseAddress.ToString(), _registrationRequest, queryParameters);

            //Act
            var request = HttpHelper.GetHttpGetRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetRequests_ShouldReturnBadRequest_WhenInvalidStatusIsSent()
        {
            //Arrange
            var registrationStatus = "invalid";
            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Registration.Status, registrationStatus }
            };
            var uriBuilder = HttpHelper.BuildUri(_httpClient.BaseAddress.ToString(), _registrationRequest, queryParameters);


            //Act
            var request = HttpHelper.GetHttpGetRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        #endregion


        #region reject-requests

        [Fact]
        public async Task RejectRequest_ShouldReturnSuccessAndUpdateRegistrationRequest_WhenValidRequestIsSent()
        {
            //Arrange
            string comment = "Duplicate request";
            var expectedResult = _registrationRequests.FirstOrDefault();

            //Act
            var result = await HttpSendRequestHelper
                .SendUpdateRequestAsync<string, RegistrationRequestDto>
                (_httpClient, _rejectRequest, expectedResult.Id, comment);

            //Assert
            Assert.Equivalent(eRegistrationStatus.Rejected.GetEnumMemberValue().ToLower(), result.RegistrationStatus.ToLower());
            Assert.Equivalent(comment, result.Review);

            Assert.NotNull(result.ReviewedAt);
            Assert.InRange(result.ReviewedAt.Value, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);

            Assert.Equivalent(expectedResult.RegistrationInfo, result.RegistrationInfo);
            Assert.Equivalent(expectedResult.CreatedAt, result.CreatedAt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenNonPositiveIdIsSent(int id)
        {
            //Arrange
            string comment = "Duplicate request";
            var expectedResult = _registrationRequests.FirstOrDefault();

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest<string>(_rejectRequest, id, comment);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenInvalidIdIsSent()
        {
            //Arrange
            int id = _dbContext.RegistrationRequests.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            string comment = "Invalid request";

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest<string>(_rejectRequest, id, comment);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        #endregion

        #region approve requests

        [Fact]
        public async Task ApproveRequest_ShouldReturnCreatedAndRegisterUser()
        {
            //Arrange
            RegistrationRequest registrationRequest = _dbContext.RegistrationRequests.FirstOrDefault(
                    x => x.RegistrationStatus == eRegistrationStatus.Pending.GetEnumMemberValue());

            Assert.Equivalent(_dbContext.UserInfo.Count(), 0);

            //Act
            var request = HttpHelper.GetHttpPostRequest<string>(_approveRequest, registrationRequest.Id,  null);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<UserInfoDto>(response);

            //Assert
            Assert.Equivalent(StatusCodes.Status200OK, response.StatusCode);

            var matchingRegistration = JsonConvert.DeserializeObject<RegistrationInfo>(registrationRequest.RegistrationInfo);
            Assert.Equivalent(matchingRegistration.UserInfo.Email, result.Email);

            //TODO: Match objects
        }

        #endregion
    }
}
