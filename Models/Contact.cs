using System.ComponentModel.DataAnnotations;

namespace JobCommandCenter.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        [Required]
        public string Platform { get; set; } = string.Empty;

        public string ProfileLink { get; set; } = string.Empty;
        public string RelationshipStatus { get; set; } = string.Empty;
        public DateTime LastContactedDate { get; set; }
        public DateTime NextFollowUpDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
