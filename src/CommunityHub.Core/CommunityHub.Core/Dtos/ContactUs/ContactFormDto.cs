using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class ContactFormDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
