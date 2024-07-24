using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs
{
    public class UpdateUserStatusDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Status must be between 1 and 10 characters.")]
        public string Status { get; set; }
    }
}
