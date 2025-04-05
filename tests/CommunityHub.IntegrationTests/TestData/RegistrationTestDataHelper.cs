using CommunityHub.Infrastructure.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class RegistrationTestDataHelper
{
    public static RegistrationInfo GetTestRegistrationInfo()
    {
        return new RegistrationInfo
        {
            UserInfo = new UserInfo
            {
                FullName = "Test User",
                Email = "testuser@example.com",
                CountryCode = "US",
                ContactNumber = "1234567890",
                Gender = "Male",
                Location = "New York",
                MaritalStatus = "Married",
                HomeTown = "Somewhere",
                SpouseInfo = new SpouseInfo
                {
                    FullName = "Spouse Name",
                    Email = "spouse@example.com",
                    CountryCode = "US",
                    ContactNumber = "0987654321",
                    Gender = "Female",
                    Location = "New York",
                    HomeTown = "Somewhere"
                },
                Children = new List<Child>
                {
                    new Child { Name = "Child One" },
                    new Child { Name = "Child Two" }
                }
            }
        };
    }

    public static RegistrationRequest GetTestRegistrationRequest()
    {
        var registrationData = GetTestRegistrationInfo();
        var registrationInfo = JsonConvert.SerializeObject(registrationData);
        return new RegistrationRequest(registrationInfo);
    }

    public static RegistrationRequest GetUpdatedTestRegistrationRequest(int id)
    {
        var request = GetTestRegistrationRequest();
        request.Id = id;
        request.Approve();
        return request;
    }
}
