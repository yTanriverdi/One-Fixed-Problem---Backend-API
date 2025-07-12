using OFP_CORE.Enums;

namespace OFB_API.VM_s.AnswerVMs
{
    public class DailyAnswerVM
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public int Likes { get; set; }

        public DateTime AnswerDate { get; set; }
        public DateTime EndAnswerDate { get; set; }

        public bool? EndAnswer { get; set; } = false;
        public bool Report { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string UserId { get; set; }
        public int ProblemId { get; set; }

        public int Comments { get; set; }

    }
}
