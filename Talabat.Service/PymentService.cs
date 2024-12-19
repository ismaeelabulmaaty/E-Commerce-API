using Microsoft.Extensions.Configuration;
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
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PymentService : IpymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePymentIntent(string BasketId)
        {
            //ApiKey =SecritKey

            StripeConfiguration.ApiKey = _configuration["StripKey:Secretkey"];
            //get basket
            var basket =await _basketRepository.GetBasketAsync(BasketId);
            if (basket is null) return null;
            var ShipingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                 ShipingPrice = deliveryMethod.Cost;

            }

            //Total  =subtotal +DeliveryMeyhodCost
            if(basket.Item.Count > 0)
            {
                
                foreach (var item in basket.Item) 
                {

                    var product =await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if(item.Price!=product.Price)
                        item.Price = product.Price;
                }

            }

            var subTotal = basket.Item.Sum(item=> item.Price * item.Quantity);
            //creat payment intent
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(basket.PymentIntentId))//create
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + ShipingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "Card" }
                };
                paymentIntent= await service.CreateAsync(option);
                basket.PymentIntentId = paymentIntent.Id;
                basket.ClintsSecrit = paymentIntent.ClientSecret;
            }
            else //update
            {

                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + ShipingPrice * 100)
                };
                paymentIntent =await service.UpdateAsync(basket.PymentIntentId, option);
                basket.PymentIntentId = paymentIntent.Id;
                basket.ClintsSecrit = paymentIntent.ClientSecret;

            }
             

           await _basketRepository.UpdateBasketAsync(basket);
            return basket;

        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrfailed(string PaymentIntent, bool flag)
        {
            var spec = new OrderWithPaymentSpecifications(PaymentIntent);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(flag)
            {
                order.Status = OrderStatus.PaymentSucceeded;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
