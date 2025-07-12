namespace OFB_API.VM_s.User
{
    public class LoginUserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }
    }
}
