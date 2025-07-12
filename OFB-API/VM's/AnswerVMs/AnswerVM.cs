using OFP_CORE.Entities;

namespace OFB_API.VM_s.AnswerVMs
{
    public class AnswerVM
    {
        public int Id { get; set; }
        public BaseUser User { get; set; }
        public string AnswerContent { get; set; }
        public DateTime AnswerDate { get; set; }
        public IEnumerable<Comment> AnswerComments { get; set; }
        public int Likes { get; set; }
        public int CommentCount { get; set; }

    }
}
