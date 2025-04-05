using CommunityHub.Infrastructure.Models;

namespace CommunityHub.IntegrationTests
{
    public static class FamilyPictureList
    {
        public static List<FamilyPicture> EmptyPictureList = new List<FamilyPicture>();

        public static List<FamilyPicture> PictureList = new List<FamilyPicture>()
        { 
            new FamilyPicture()
            {
                Id = 1,
                UserInfoId = 2,
                ImageUrl = "http://localhost.com/wwwroot/images/2/image.png"
            },
            new FamilyPicture()
            {
                Id = 2,
                UserInfoId = 23,
                ImageUrl = "http://localhost.com/wwwroot/images/23/image.png"
            },
            new FamilyPicture()
            {
                Id = 3,
                UserInfoId = 25,
                ImageUrl = "http://localhost.com/wwwroot/images/25/image.png"
            }
        };
    }
}
