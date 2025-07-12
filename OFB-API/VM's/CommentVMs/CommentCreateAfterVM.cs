using OFP_CORE.Entities;
using OFP_CORE.Enums;

namespace OFB_API.VM_s.CommentVMs
{
    public class CommentCreateAfterVM
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public int AnswerId { get; set; }
        public string UserId { get; set; }
    }
}
