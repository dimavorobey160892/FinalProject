using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.Models
{
    public class CallRequestModel
    {
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your phone number")]
        [RegularExpression(@"^\+?3?8?(0\d{9})$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }
}
