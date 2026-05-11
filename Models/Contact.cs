using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

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
        public RelationshipStatus RelationshipStatus { get; set; } = RelationshipStatus.NotContacted;
        public DateTime? LastContactedDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
