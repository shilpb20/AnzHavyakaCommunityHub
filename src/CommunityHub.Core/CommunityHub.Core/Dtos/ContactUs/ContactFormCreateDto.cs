using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class ContactFormCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Location { get; set; }


        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}
