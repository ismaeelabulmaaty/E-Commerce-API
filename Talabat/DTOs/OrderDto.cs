using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Orders;

namespace Talabat.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethod { get; set; }
        public AddressUserDto ShippingAddress { get; set; }
    }
}
