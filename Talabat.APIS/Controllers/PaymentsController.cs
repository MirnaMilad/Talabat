using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.core.Services;

namespace Talabat.APIS.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_1a1eeb289a11244bb0513fc36cea0057d9ed257d0c619c3dc454fc85364a9300";

        public PaymentsController(IPaymentServices paymentServices , IMapper mapper)
        {
            _paymentServices = paymentServices;
            _mapper = mapper;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent (string basketId)
        {
            var CustomerBasket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if(CustomerBasket is null) {
                return BadRequest(new ApiResponse(400, "There is a problem with your basket"));

                    
                    }

            var MappedBasket = _mapper.Map<CustomerBasketDto>(CustomerBasket);
            return Ok(MappedBasket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    await _paymentServices.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id , false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentServices.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
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
