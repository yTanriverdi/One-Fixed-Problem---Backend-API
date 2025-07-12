using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.UpdateDTOs.Likes
{
    public class AnswerLikeUpdateDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AnswerId { get; set; }
    }
}
