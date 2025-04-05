using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class ChildDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}