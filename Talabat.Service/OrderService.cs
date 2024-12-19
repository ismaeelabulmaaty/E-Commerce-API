using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecification;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IpymentService _pymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork , IpymentService pymentService)
        { 
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _pymentService = pymentService;
        }

       
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Core.Entities.Orders.Address ShippingAddress)
        {

            //Get Basket From Basket Repo
            var Basket =await _basketRepository.GetBasketAsync(basketId);
            // Get Seelected Items at basket from productRepo
            var OrderItems = new List<OrderItem>();

            if(Basket?.Item.Count > 0)
            {
                foreach(var item in Basket.Item)
                {
                    var product = await _unitOfWork.Repository<Core.Entities.Product>().GetAsync(item.Id);
                    var productItemOrder = new ProductItemOrder(product.Id , product.Name , product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrder, item.Quantity , (int)product.Price);
                    OrderItems.Add(orderItem);
                }
            }

            //calc subtotal
            var subTotal = OrderItems.Sum(item=>item.Price *  item.Quantity);

            //Get DeliveryMethod From DeliveryMethodRepo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            //create Order
            var spec = new OrderWithPaymentSpecifications(Basket.PymentIntentId);
            var ExOrder =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().delete(ExOrder);
                await _pymentService.CreateOrUpdatePymentIntent(basketId);


            }

            // var Order = new Order(buyerEmail , ShippingAddress , deliveryMethod , OrderItems , subTotal   ,Basket.PymentIntentId);


            var order = new Order()
            {

                BuyerEmail = buyerEmail,
                ShippingAddress =ShippingAddress,
                DeliveryMethod = deliveryMethod,
                Items = OrderItems,
                SubTotal = subTotal,
                PaymentIntentId = Basket.PymentIntentId,

            };
           await _unitOfWork.Repository<Order>().AddAsync(order);

           var result= await _unitOfWork.CompleteAsync();
            if (result <= 0) return null; 
            return order;
        }

        public async Task<Order> GetOrderByIdForSpecificUsserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail , orderId);
            var Order =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUsserAsync(string BuyerEmail)
        {
           
            var spec = new OrderSpecification(BuyerEmail);
            var Orders =await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethod;
        }

       
    }
}
