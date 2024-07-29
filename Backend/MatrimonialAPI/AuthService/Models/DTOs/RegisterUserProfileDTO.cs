using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class RegisterUserProfileDTO
    {
        public int UserId { get; set; }
        public string ProfileFor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
