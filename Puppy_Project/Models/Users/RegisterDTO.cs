using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Models.Input_OutputDTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        public string Password { get; set; }
        public string Place { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }
    }
}
