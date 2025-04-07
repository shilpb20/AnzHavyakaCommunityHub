using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Infrastructure.Models
{
    public class ContactForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [EmailAddress]
        [Required]
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

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}