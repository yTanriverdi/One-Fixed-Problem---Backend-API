using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs
{
    public class SuggestCreateDTO
    {
        public string Email { get; set; }
        public string Name { get; set; } // Kullanıcının ismi

        [Required(ErrorMessage = "Content is required")]
        [MinLength(4, ErrorMessage = "Minimum 4 character")]
        [MaxLength(400, ErrorMessage = "Minimum 400 character")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Content must contain only letters and spaces.")]
        public string Content { get; set; }
        public bool IsSuggest { get; set; } = true; // ÖNERİ Mİ ŞİKAYET Mİ ?
    }
}
