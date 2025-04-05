namespace CommunityHub.Core.Dtos
{
    public class FamilyPictureUpdateDto
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }

        public int UserId { get; set; }
    }
}
