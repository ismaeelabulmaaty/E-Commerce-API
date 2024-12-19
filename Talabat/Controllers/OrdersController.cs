using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services.Contract;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Service;

namespace Talabat.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper  _mapper;


        public OrdersController(IOrderService orderService , IMapper mapper )
        {
            _orderService = orderService;
            _mapper = mapper;

        }


        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CretateOrder(OrderDto orderDto)
        {
            var BuyerEmail =  User.FindFirstValue(ClaimTypes.Email);
            var Address = _mapper.Map<AddressUserDto , Address>(orderDto.ShippingAddress);
            var Order = _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId , orderDto.DeliveryMethod , Address);
            if (Order is null) return BadRequest(new ApiResponse(400, "There is a problem with order"));
            return Ok(Order);

        }



        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderForSpecificUsserAsync(buyerEmail);
            if (order is null) return NotFound( new ApiResponse(404 , "There is no order for this user"));
            var MappedOrder = _mapper.Map <IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(order);
            return Ok(MappedOrder);

        }

        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser( int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderByIdForSpecificUsserAsync(buyerEmail, id);
            if (Orders is null) return NotFound(new ApiResponse(404, $"There is no order with id = {id} for this user"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(Orders);
            return Ok(MappedOrder);
                   


        }
    
         
        [HttpGet("DeliveryMethods")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {

            var deliveryMethod = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethod);

        }



    }
}
