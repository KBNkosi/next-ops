using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.FollowUps
{
    public class CreateFollowUpRequest
    {

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public FollowUpType FollowUpType { get; set; } = FollowUpType.GeneralTask;
        public int? ApplicationId { get; set; }
        public int? ContactId { get; set; }
        public string Notes { get; set; } = string.Empty;

    }
 
}