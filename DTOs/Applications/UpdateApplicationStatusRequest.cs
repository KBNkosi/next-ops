using JobCommandCenter.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobCommandCenter.DTOs.Applications
{
     public class UpdateApplicationStatusRequest
     {
          [EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Status must be a valid ApplicationStatus value")]
          public ApplicationStatus Status { get; set; } = ApplicationStatus.Saved;
     }
}
