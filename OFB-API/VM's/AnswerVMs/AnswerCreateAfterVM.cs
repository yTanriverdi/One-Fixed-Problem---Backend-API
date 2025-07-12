using OFP_CORE.Enums;

namespace OFB_API.VM_s.AnswerVMs
{
    public class AnswerCreateAfterVM
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }

        public DateTime AnswerDate { get; set; }
        public DateTime EndAnswerDate { get; set; }

        public bool? EndAnswer { get; set; } = false;

        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
    }
}
