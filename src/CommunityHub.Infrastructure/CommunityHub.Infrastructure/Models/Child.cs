using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Infrastructure.Models
{
    public class Child
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get; set; }
        
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public int UserInfoId { get; set; }

        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }
    }
}