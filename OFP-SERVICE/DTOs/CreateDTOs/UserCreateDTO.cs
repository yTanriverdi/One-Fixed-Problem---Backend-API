using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [MinLength(3, ErrorMessage = "Minimum 3 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "First Name must contain only letters and spaces.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Minimum 2 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Last Name must contain only letters and spaces.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [EmailAddress(ErrorMessage = "Please Email Format")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum 6 character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "CheckPassword is required")]
        [MinLength(6,ErrorMessage = "Minimum 6 character")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string CheckPassword { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(2, ErrorMessage = "Minimum 2 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Username must contain only letters and spaces.")]
        public string UserName { get; set; }
    }
}
