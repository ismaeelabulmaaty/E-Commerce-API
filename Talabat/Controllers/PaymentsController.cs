using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.DTOs;
using Talabat.Errors;

namespace Talabat.Controllers
{
    
    public class PaymentsController : BaseApiController
    {
        
        private readonly IpymentService _pymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_3d5244002fd404cd35e24302007fc09740098642c41f8e3ef83a75f0d5e562b2";

        public PaymentsController(IpymentService pymentService , IMapper mapper)
        {
            _pymentService = pymentService;
            _mapper = mapper;
        }
        //creat or update 
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status400BadRequest)]
        [HttpPost("CreateOrUpdatePaymentIntent/{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {


            var customerBasket=  await   _pymentService.CreateOrUpdatePymentIntent(basketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400 , "There is a proplem with your basket"));
            var MappedBasket = _mapper.Map< CustomerBasket,CustomerBasketDto>(customerBasket);
            return Ok(MappedBasket);
            


        }


        [HttpPost("StripeWebHook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);


                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                 await   _pymentService.UpdatePaymentIntentToSucceedOrfailed(paymentIntent.Id , false);

                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _pymentService.UpdatePaymentIntentToSucceedOrfailed(paymentIntent.Id, true);
                }
               

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
