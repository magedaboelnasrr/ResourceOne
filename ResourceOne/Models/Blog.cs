using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceOne.Models
{
    public class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }        
        public DateTime CreatedDate { get; set; }
        public string Category { get; set; }
        public string ImageName { get; set; }
        public string Publisher { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
