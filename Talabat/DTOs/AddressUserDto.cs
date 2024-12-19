using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Talabat.Core.Entities.Identity;

namespace Talabat.DTOs
{
    public class AddressUserDto
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
      

    }
}
