using JobCommandCenter.Enums;

public class UpdateApplicationStatusRequest
{
     public ApplicationStatus Status { get; set; } = ApplicationStatus.Saved;
}