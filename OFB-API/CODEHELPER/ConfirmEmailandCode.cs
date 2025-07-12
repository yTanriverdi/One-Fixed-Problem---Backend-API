using System.ComponentModel.DataAnnotations;

namespace OFB_API.CODEHELPER
{
    public class ConfirmEmailandCode
    {
        [EmailAddress(ErrorMessage = "Please Email format")]
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
