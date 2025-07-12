namespace OFB_API.VM_s.AnswerUsers
{
    public class AnswerUserVM
    {
        public string AnswerContent { get; set; }
        public int Likes { get; set; }
        public DateTime AnswerDate { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
    }
}
