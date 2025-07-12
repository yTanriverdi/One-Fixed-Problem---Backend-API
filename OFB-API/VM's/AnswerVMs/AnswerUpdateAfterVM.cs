using OFP_CORE.Entities;

namespace OFB_API.VM_s.AnswerVMs
{
    public class AnswerUpdateAfterVM
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public BaseUser BaseUser { get; set; }
    }
}
