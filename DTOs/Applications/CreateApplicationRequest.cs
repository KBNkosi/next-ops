using System.ComponentModel.DataAnnotations;
using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.Applications
{
    public class CreateApplicationRequest
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string RoleTitle { get; set; } = string.Empty;

        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Status must be a valid ApplicationStatus value")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Saved;

        public string Notes { get; set; } = string.Empty;

    }
}
