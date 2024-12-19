using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deleveryMethodId, Address ShippingAddress);

        Task<IReadOnlyList<Order>> GetOrderForSpecificUsserAsync(string email);

        Task<Order> GetOrderByIdForSpecificUsserAsync(string buyerEmail , int orderId);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
       

    }
}
