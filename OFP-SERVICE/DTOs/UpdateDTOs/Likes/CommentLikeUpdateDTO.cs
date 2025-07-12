using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.UpdateDTOs.Likes
{
    public class CommentLikeUpdateDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
