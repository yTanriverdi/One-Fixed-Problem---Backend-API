using OFP_CORE.Entities;
using OFP_CORE.Enums;

namespace OFB_API.VM_s.User
{
    public class UserDetailsVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Region { get; set; }
        public string Gender { get; set; }
        public int Likes { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public Badge Badge { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}
