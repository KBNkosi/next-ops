namespace JobCommandCenter.DTOs.Applications
{
    public class UpdateApplicationRequest
    {
        public string? CompanyName { get; set; } = string.Empty;

        public string? RoleTitle { get; set; } = string.Empty;

        public string? Platform { get; set; } = string.Empty;

        public string? JobLink { get; set; } = string.Empty;

        public string? Notes { get; set; } = string.Empty;

    }
}
