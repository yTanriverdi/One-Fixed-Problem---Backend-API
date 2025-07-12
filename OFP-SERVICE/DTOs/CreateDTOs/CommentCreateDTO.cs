using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs
{
    public class CommentCreateDTO
    {
        [Required(ErrorMessage = "Content is required")]
        [MinLength(4, ErrorMessage = "Minimum 4 character")]
        [MaxLength(350, ErrorMessage = "Maximum 350 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Content must contain only letters and spaces.")]
        public string CommentContent { get; set; }
        public int AnswerId { get; set; }
        public string UserId { get; set; }
    }
}
