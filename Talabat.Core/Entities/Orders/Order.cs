using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Entities.Orders
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        //public Order(string buyerEmail, OrderStatus status, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        //{
        //    BuyerEmail = buyerEmail;
        //    Status = status;
        //    ShippingAddress = shippingAddress;
        //    DeliveryMethod = deliveryMethod;
        //    Items = items;
        //    SubTotal = subTotal;
        //    PaymentIntentId = paymentIntentId;
        //}

        //public Order(string buyerEmail, Address shippingAddress, DeliveryMethod? deliveryMethod, List<OrderItem> orderItems, decimal subTotal, string pymentIntentId)
        //{
           
           
        //}

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }
        //public int DeliveryMethodId { get; set; }//forignKey[1:1]
        public DeliveryMethod DeliveryMethod { get; set; } // Navigation Prop Of [One]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();// Navigation Prop Of [Many]
        public decimal SubTotal { get; set; }
        //[NotMapped]
        //public decimal Total { get; set; } // subTotal + DeliveryMethod.cost

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; }
    }
}
