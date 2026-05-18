using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.Models
{
    public class FollowUp
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        public bool Completed { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        [Required]
        public FollowUpType FollowUpType { get; set; } = FollowUpType.GeneralTask;
        public Application? Application { get; set; }
        public int? ApplicationId { get; set; }
        public Contact? Contact { get; set; }
        public int? ContactId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
