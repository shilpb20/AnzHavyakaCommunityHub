using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class FamilyPictureCreateDto
    {
        [Required]
        public string ImageUrl { get; set; }
    }
}
