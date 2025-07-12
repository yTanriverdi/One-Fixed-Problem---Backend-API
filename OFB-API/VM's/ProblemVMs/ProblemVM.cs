using OFP_CORE.Entities;

namespace OFB_API.VM_s.ProblemVMs
{
    public class ProblemVM
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int AnswerCount { get; set; }
        public DateTime ProblemDate { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}
