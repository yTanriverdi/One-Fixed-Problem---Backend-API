using System.ComponentModel.DataAnnotations;

namespace OFB_API.VM_s.User
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Password { get; set; }
    }
}
