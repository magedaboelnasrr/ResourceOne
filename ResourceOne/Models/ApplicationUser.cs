using Microsoft.AspNetCore.Identity;

namespace ResourceOne.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
