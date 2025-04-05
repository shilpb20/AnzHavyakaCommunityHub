using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Models
{
    public class FamilyPicture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? ImageUrl { get; set; }

        public int UserInfoId { get; set; }

        [ForeignKey(nameof(UserInfoId))]
        public UserInfo UserInfo { get; set; }
    }
}
