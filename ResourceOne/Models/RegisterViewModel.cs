using System.ComponentModel.DataAnnotations;

namespace ResourceOne.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), MinLength(3)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), MinLength(3), Compare("Password"), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
