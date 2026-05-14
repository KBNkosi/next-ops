using System.ComponentModel.DataAnnotations;


namespace JobCommandCenter.DTOs.Applications
{
    public class UpdateApplicationRequest
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
}
