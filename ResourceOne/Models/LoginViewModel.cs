using System.ComponentModel.DataAnnotations;

namespace ResourceOne.Models
{
    public class LoginViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), MinLength(3)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
