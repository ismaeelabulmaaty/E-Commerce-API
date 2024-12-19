using System.ComponentModel.DataAnnotations;

namespace Talabat.DTOs
{
    public class loginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
