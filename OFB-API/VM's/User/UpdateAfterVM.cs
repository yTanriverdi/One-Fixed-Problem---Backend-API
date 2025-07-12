using System.ComponentModel.DataAnnotations;

namespace OFB_API.VM_s.User
{
    public class UpdateAfterVM
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Region { get; set; }
        public string Gender { get; set; }
    }
}
