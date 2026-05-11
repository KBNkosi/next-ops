using System.ComponentModel.DataAnnotations;


public class UpdateContactRequest
{
       [Required]
        public string Name { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        [Required]
        public string Platform { get; set; } = string.Empty;

        public string ProfileLink { get; set; } = string.Empty;
     
        public DateTime? NextFollowUpDate { get; set; }
        public string Notes { get; set; } = string.Empty;
}