using System.ComponentModel.DataAnnotations;

namespace OFB_API.DTO_s.User
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }
    }
}
