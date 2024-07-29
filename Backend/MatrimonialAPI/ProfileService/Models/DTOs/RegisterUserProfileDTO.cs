using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models.DTOs
{
    public class RegisterUserProfileDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ProfileFor { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
    }
}
