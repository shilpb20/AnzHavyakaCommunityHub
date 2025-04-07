using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class UserInfoUpdateDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
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

        [Required]
        public string MaritalStatus { get; set; } = string.Empty;

        public string? HomeTown { get; set; } = string.Empty;

        public string? HouseName { get; set; }
    }
}