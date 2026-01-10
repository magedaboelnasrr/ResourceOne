using System.ComponentModel.DataAnnotations;

namespace ResourceOne.Models
{
    public class ApplicationFormViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string YearsOfExperience { get; set; }
        [Required]
        public string CurrentPosition { get; set; }
        public string CVName { get; set; }
        public IFormFile CV { get; set; }
    }
}
