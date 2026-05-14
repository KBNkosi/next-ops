using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.Applications
{
     public class UpdateApplicationStatusRequest
     {
          public ApplicationStatus Status { get; set; } = ApplicationStatus.Saved;
     }
}
