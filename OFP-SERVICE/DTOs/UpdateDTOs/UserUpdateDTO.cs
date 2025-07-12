using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.UpdateDTOs
{
    public class UserUpdateDTO
    {
        public string? Id { get; set; }


        [Required(ErrorMessage = "First Name is required")]
        [MinLength(3,ErrorMessage = "Minimum 3 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "First Name must contain only letters and spaces.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Minimum 2 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Last Name must contain only letters and spaces.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Username is required")]
        [MinLength(2, ErrorMessage = "Minimum 2 character")]
        [MaxLength(30, ErrorMessage = "Maximum 30 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Username must contain only letters and spaces.")]
        public string UserName { get; set; }


        [Required]
        public string Region { get; set; }


        [Required]
        public string Gender { get; set; }
    }
}
