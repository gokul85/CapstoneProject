using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class UserDetails
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordHashKey { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public bool IsPremium { get; set; }
    }
}
