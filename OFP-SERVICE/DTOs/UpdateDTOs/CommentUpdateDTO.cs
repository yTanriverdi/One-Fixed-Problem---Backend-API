using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.UpdateDTOs
{
    public class CommentUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(3, ErrorMessage = "Minimum 3 character.")]
        [MaxLength(350, ErrorMessage = "Maximum 350 character.")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Content must contain only letters and spaces.")]
        public string CommentContent { get; set; }
    }
}
