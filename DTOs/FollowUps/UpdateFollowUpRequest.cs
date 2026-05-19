using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.FollowUps
{
    public class UpdateFollowUpRequest
    {
        public string? Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public FollowUpType? FollowUpType { get; set; } = Enums.FollowUpType.GeneralTask;
        public int? ApplicationId { get; set; }
        public int? ContactId { get; set; }

        public Outcome? Outcome = Enums.Outcome.None;
        public string? Notes { get; set; } = string.Empty;
    }
}
