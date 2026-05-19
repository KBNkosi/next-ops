using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.FollowUps       
{
    public class FollowUpResponse
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        [Required]
        public FollowUpType FollowUpType { get; set; } = FollowUpType.GeneralTask;        
        public int? ApplicationId { get; set; }       
        public int? ContactId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public Outcome Outcome { get; set; } = Outcome.None; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
