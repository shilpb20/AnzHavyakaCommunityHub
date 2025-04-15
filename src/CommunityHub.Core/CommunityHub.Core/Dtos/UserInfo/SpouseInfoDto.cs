using System.ComponentModel.DataAnnotations;

namespace CommunityHub.Core.Dtos
{
    public class SpouseInfoDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? HomeTown { get; set; } = string.Empty;
        public string? HouseName { get; set; } = string.Empty;
    }
}