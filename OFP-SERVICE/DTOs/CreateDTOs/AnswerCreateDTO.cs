using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs
{
    public class AnswerCreateDTO
    {
        [Required(ErrorMessage = "Answer Content is required")]
        [MinLength(4, ErrorMessage = "Minimum 4 character")]
        [MaxLength(500, ErrorMessage = "Maximum 500 character")]
        //[RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Answer must contain only letters and spaces.")]
        [RegularExpression("^[a-zA-Z\\s.,]+$", ErrorMessage = "Answer must contain only letters, spaces, dots, and commas.")]
        public string AnswerContent { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
    }
}
