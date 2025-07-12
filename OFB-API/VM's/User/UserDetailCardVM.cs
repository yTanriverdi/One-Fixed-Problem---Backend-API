using OFP_CORE.Enums;

namespace OFB_API.VM_s.User
{
    public class UserDetailCardVM
    {
        public string UserName { get; set; }
        public int Likes { get; set; }
        public int AnswerCount { get; set; }
        public Badge Badge { get; set; }
    }
}
