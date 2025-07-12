namespace OFB_API.VM_s.CommentVMs
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Likes { get; set; }
        public int AnswerId { get; set; }
    }
}
