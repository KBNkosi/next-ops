using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.Applications
{
     public class ApplicationResponse
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string RoleTitle { get; set; } = string.Empty;

        [Required]
        public string Platform { get; set; } = string.Empty;

         public string JobLink { get; set; } = string.Empty;

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Saved;

        public DateTime? DateApplied { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}


   
