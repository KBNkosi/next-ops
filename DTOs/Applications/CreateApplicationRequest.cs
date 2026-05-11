using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

public class CreateApplicationRequest
{
    [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string RoleTitle { get; set; } = string.Empty;

        [Required]
        public string Source { get; set; } = string.Empty;

        public string JobLink { get; set; } = string.Empty;
        
        public DateTime? FollowUpDate { get; set; }
        public string Notes { get; set; } = string.Empty;

}