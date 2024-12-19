using System.ComponentModel.DataAnnotations;

namespace Talabat.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhonNumber { get; set; }
        [Required]
        //[RegularExpression()]
        public string Password { get; set; }
    }
}
