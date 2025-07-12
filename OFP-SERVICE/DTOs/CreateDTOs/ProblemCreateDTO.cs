using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs
{
    public class ProblemCreateDTO
    {
        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Content minimum 10 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Content must contain only letters and spaces.")]
        public string Content { get; set; }
    }
}
