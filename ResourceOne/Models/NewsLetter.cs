using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceOne.Models
{
    public class NewsLetter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
