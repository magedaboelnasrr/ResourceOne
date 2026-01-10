using System.ComponentModel.DataAnnotations;

namespace ResourceOne.Models
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }        
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
    }
}
