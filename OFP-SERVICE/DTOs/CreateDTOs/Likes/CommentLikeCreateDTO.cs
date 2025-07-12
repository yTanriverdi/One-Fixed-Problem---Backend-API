using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.CreateDTOs.Likes
{
    public class CommentLikeCreateDTO
    {
        public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
