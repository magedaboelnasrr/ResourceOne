namespace ResourceOne.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; }


        // اسم المستخدم من Identity
        public string UserName { get; set; }

        // Id الخاص بالمستخدم من Identity (اختياري)
        public string UserId { get; set; }
    }
}
