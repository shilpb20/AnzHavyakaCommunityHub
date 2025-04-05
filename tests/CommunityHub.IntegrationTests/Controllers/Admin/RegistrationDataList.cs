using CommunityHub.Core.Dtos;

namespace CommunityHub.IntegrationTests.TestData
{
    public static class RegistrationDataList
    {
        public static List<RegistrationInfoCreateDto> GetRequests()
        {
            return new List<RegistrationInfoCreateDto>()
            {
                new RegistrationInfoCreateDto()
                {
                    UserInfo = new UserInfoCreateDto()
                    {
                        FullName = "Alice Springs",
                        Email = "alice.springs@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456789",
                        Gender = "Female",
                        MaritalStatus = "Unmarried",
                        Location = "Northern Territory",
                        HomeTown = "Atlanta",
                        HouseName = "Naomi home"
                    }
                },
                new RegistrationInfoCreateDto()
                {
                    UserInfo = new UserInfoCreateDto()
                    {
                        FullName = "Alex Springs",
                        Email = "alex.springs@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456790",
                        Gender = "Male",
                        MaritalStatus = "Married",
                        Location = "Southern Australia",
                        HomeTown = "Atlanta",
                        HouseName = "Naomi home"
                    },
                    SpouseInfo = new SpouseInfoCreateDto()
                    {
                        FullName = "Alicia Adams",
                        Email = "alicia.adams@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123123412",
                        HomeTown = "Georgia",
                        HouseName = "Adams Family",
                        Gender = "Female",
                        Location = "New Castle",
                    },
                    Children = new List<ChildrenCreateDto>()
                    {
                         new() { Name = "Anna", },
                         new() { Name = "Ben"}, 
                         new() { Name = "Cairo" }
                    }
                },
                new RegistrationInfoCreateDto()
                {
                    UserInfo = new UserInfoCreateDto()
                    {
                        FullName = "Alice Smith",
                        Email = "alice.smith@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456756",
                        Gender = "Female",
                        MaritalStatus = "Unmarried",
                        Location = "New South Whales",
                        HomeTown = "Sydney",
                        HouseName = "Smith Family"
                    },
                    Children = new List<ChildrenCreateDto>() 
                    { 
                        new() { Name = "Ari" },
                        new() { Name = "Garry" },
                        new() { Name = "Cherry" },
                        new() { Name = "Fairy" }
                    }
                },
                new RegistrationInfoCreateDto()
                {
                    UserInfo = new UserInfoCreateDto()
                    {
                        FullName = "Daisy Shaw",
                        Email = "daisy.shaw@gmail.com",
                        CountryCode = "+61",
                        ContactNumber = "0123456742",
                        Gender = "Female",
                        MaritalStatus = "Widow",
                        Location = "New South Whales",
                        HomeTown = "Blue Mountains",
                        HouseName = "Shaw Clan"
                    },
                }
            };
        }
    }
}
