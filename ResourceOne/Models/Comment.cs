namespace ResourceOne.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        // اسم المستخدم من Identity
        public string UserName { get; set; }

        // Id الخاص بالمستخدم من Identity (اختياري)
        public string UserId { get; set; }
    }
}
