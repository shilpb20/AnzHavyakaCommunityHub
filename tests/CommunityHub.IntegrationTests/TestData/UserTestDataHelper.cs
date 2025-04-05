using CommunityHub.Infrastructure.Models;

namespace CommunityHub.Tests.Data
{
    public static class UserTestsDataHelper
    {
        public static List<UserInfo> GetUsers()
        {
            return new List<UserInfo>()
            {
                new UserInfo
                {
                    Id = 1,
                    FullName = "Charlie",
                    Email = "john@example.com",
                    CountryCode = "+1",
                    ContactNumber = "123456789",
                    Gender = "Male",
                    Location = "New York",
                    MaritalStatus = "Married",
                    HomeTown = "Town A",
                    HouseName = "House A",
                    ApplicationUserId = "app-user-1",
                    SpouseInfo = new SpouseInfo
                    {
                        FullName = "Jane Doe",
                        Email = "jane@example.com",
                        CountryCode = "+1",
                        ContactNumber = "987654321",
                        Gender = "Female",
                        Location = "City A",
                        HomeTown = "Town B"
                    },
                    Children = new List<Child>
                    {
                        new Child { Name = "Child One" },
                        new Child { Name = "Child Two" }
                    }
                },
                new UserInfo
                {
                    Id = 2,
                    FullName = "Alice",
                    Email = "alice@example.com",
                    CountryCode = "+44",
                    ContactNumber = "987654321",
                    Gender = "Female",
                    Location = "Los Angeles",
                    MaritalStatus = "Single",
                    HomeTown = "Town C",
                    HouseName = "House B",
                    ApplicationUserId = "app-user-2",
                    SpouseInfo = null,
                    Children = new List<Child>()
                },
                new UserInfo
                {
                    Id = 3,
                    FullName = "Bob",
                    Email = "bob@example.com",
                    CountryCode = "+33",
                    ContactNumber = "1122334455",
                    Gender = "Male",
                    Location = "Chicago",
                    MaritalStatus = "Single",
                    HomeTown = "Town D",
                    HouseName = "House C",
                    ApplicationUserId = "app-user-3",
                    SpouseInfo = null,
                    Children = new List<Child>
                    {
                        new Child { Name = "Child One" }
                    }
                }
            };
        }
    }
}
