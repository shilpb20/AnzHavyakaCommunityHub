using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Infrastructure.Models.Registration
{
    public class SpouseInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CountryCode { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;
        
        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public string? HomeTown { get; set; } = string.Empty;

        public string? HouseName { get; set; }


        public int UserInfoId { get; set; }

        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }
    }
}