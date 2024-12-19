using System.ComponentModel.DataAnnotations;

namespace Talabat.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Item { get; set; }
        public string? PymentIntentId { get; set; }
        public string? ClintsSecrit { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
