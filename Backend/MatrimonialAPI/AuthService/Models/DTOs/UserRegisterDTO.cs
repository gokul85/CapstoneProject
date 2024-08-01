using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "OnBehalf is Required")]
        public string OnBehalf { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must be a maximum of 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "DOB is Required")]
        public DateTime DOB { get; set; }
    }
}
