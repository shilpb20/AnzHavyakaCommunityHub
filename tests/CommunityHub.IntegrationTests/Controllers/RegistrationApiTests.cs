using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Helpers;
using CommunityHub.Infrastructure.Models;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CommunityHub.IntegrationTests.Controllers
{
    public class RegistrationApiTests : BaseControllerTests
    {
        string _createRequest = ApiRoute.Registration.CreateRequest;
        string _getRequestById = ApiRoute.Registration.GetRequestById;

        public RegistrationApiTests(ApplicationStartup application) : base(application)
        {
        }

        #region add-registration-request


        [Fact]
        public async Task RegisterUser_ReturnsCreatedAtResult_WhenValidDataIsSent()
        {
            var registrationDataCreateDto = GetRegistrationDataCreateDto();
            var registrationData = _mapper.Map<RegistrationInfoDto>(registrationDataCreateDto);

            var request = HttpHelper.GetHttpPostRequest(_createRequest, registrationDataCreateDto);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<RegistrationRequestDto>(response);

            Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
            Assert.NotNull(result);

            Assert.Equivalent(registrationData, result.RegistrationInfo);
            Assert.Equal(RegistrationStatusHelper.PendingStatus, result.RegistrationStatus);
            Assert.InRange(result.CreatedAt, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);
            Assert.Null(result.ReviewedAt);
            Assert.Null(result.Review);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenNullDataIsSent()
        {
            var request = HttpHelper.GetHttpPostRequest<object>(_createRequest, null);
            request.Content = new StringContent("null", Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenInvalidDataIsSent()
        {
            var invalidData = new { RandomField = "Invalid" };
            var request = HttpHelper.GetHttpPostRequest(_createRequest, invalidData);
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenDataWithMissingFieldsIsSent()
        {
            var incompleteData = new RegistrationInfoCreateDto
            {
                UserInfo = new UserInfoCreateDto
                {
                    FullName = "Alice Smith",
                    Email = "Alice.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0410203040"
                }
            };

            var request = HttpHelper.GetHttpPostRequest(_createRequest, incompleteData);
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact(Skip = "Skipping due to low priority or irrelevance at this stage")]
        public async Task RegisterUser_ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
        {
            //TODO
        }


        private async Task<RegistrationRequestDto> AddRegistrationRequest()
        {
            var registrationData = GetRegistrationDataCreateDto();
            return await HttpSendRequestHelper.SendPostRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(_httpClient, _createRequest, registrationData);
        }

        private static RegistrationInfoCreateDto GetRegistrationDataCreateDto()
        {
            return new RegistrationInfoCreateDto
            {
                UserInfo = new UserInfoCreateDto
                {
                    FullName = "Alice Smith",
                    Email = "Alice.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0410203040",
                    Location = "Sydney",
                    Gender = "Female",
                    MaritalStatus = "Married",
                    HomeTown = "Beranki",
                    HouseName = "Kaadu mane"
                },
                SpouseInfo = new SpouseInfoCreateDto
                {
                    FullName = "Janet Smith",
                    Email = "janet.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0420202020",
                    Gender = "Male",
                    Location = "New South Whales",
                    HomeTown = "Sydney",
                    HouseName = "Smith Family"
                },
                Children = new List<ChildrenCreateDto>
            {
                new() { Name = "Anna" },
                new() { Name = "Benny" },
                new() { Name = "Cairo" }
            }
            };
        }

        #endregion

        //#region get-registration-request

        //[Fact]
        //public async Task GetRegistrationRequest_ReturnsNotFound_WhenDataIsEmpty()
        //{
        //    //Arrange
        //    await ClearRegistrationRequestsAsync();

        //    //Act
        //    var request = HttpHelper.GetHttpGetRequestById(_getRequestById, 1);
        //    var response = await _httpClient.SendAsync(request);

        //    //Assert
        //    Assert.Equivalent(StatusCodes.Status404NotFound, response.StatusCode);
        //}

        //[Fact]
        //public async Task ClearRegistrationRequestsAsync()
        //{
        //    var allRequests = _dbContext.RegistrationRequests.ToList();
        //    _dbContext.RegistrationRequests.RemoveRange(allRequests);
        //    await _dbContext.SaveChangesAsync();
        //}

        //[Theory]
        //[InlineData(0)]
        //[InlineData(-1)]
        //public async Task GetRegistrationRequest_ReturnsBadRequest_WhenNonPositiveIdIsSent(int id)
        //{
        //    //Act
        //    var request = HttpHelper.GetHttpGetRequestById(_getRequestById, id);
        //    var response = await _httpClient.SendAsync(request);

        //    //Assert
        //    Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task GetRegistrationRequest_ReturnsMatchingResult_WhenValidIdIsSent()
        //{
        //    //Arrange
        //    var registrationRequest = await AddRegistrationRequest();

        //    //Act
        //    var request = HttpHelper.GetHttpGetRequestById(_getRequestById, registrationRequest.Id);
        //    var response = await _httpClient.SendAsync(request);
        //    var result = await HttpHelper.GetHttpResponseObject<RegistrationRequestDto>(response);

        //    //Assert
        //    Assert.Equivalent(StatusCodes.Status200OK, response.StatusCode);
        //    Assert.Equivalent(registrationRequest, result);
        //}

        //#endregion
    }
}
