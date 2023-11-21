using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core;
using Talabat.core.Entities;
using Talabat.core.Entities.OrderAggrigation;
using Talabat.core.Repositories;
using Talabat.core.Services;
using Talabat.core.Specifications.OrderSpec;

namespace Talabat.services
{
    public class PaymentService : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            //Secret Key
            StripeConfiguration.ApiKey = _configuration["Stripekeys:Secretkey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            var shippingPrice = 0M;
            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                var ShippingPrice = DeliveryMethod.Cost;
            }
            if(Basket.Items.Count> 0)
            {
                foreach (var item in Basket.Items)
                {
                    //var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var Product = await _unitOfWork.Repository<Products>().GetByIdAsync(item.Id);
                    if(item.Price != Product.Price)
                    {
                        item.Price = Product.Price;
                    }
                    var Subtotal = Basket.Items.Sum(item => item.Price * item.Quantity);
                    //Create Payment Intent

                    var Service = new PaymentIntentService();
                    PaymentIntent paymentIntent;
                    if (string.IsNullOrEmpty(Basket.PaymentIntentId)) //Create
                    {
                        var Options = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)(Subtotal * 100 + shippingPrice * 100),
                            Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card"}

                        };
                        paymentIntent = await Service.CreateAsync(Options); 
                        Basket.PaymentIntentId = paymentIntent.Id;
                        Basket.ClientSecret = paymentIntent.ClientSecret;
                    }
                    else //Update
                    {
                        var Options = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)(Subtotal * 100 + shippingPrice * 100),

                        };
                        paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);

                        Basket.PaymentIntentId = paymentIntent.Id;
                        Basket.ClientSecret = paymentIntent.ClientSecret;
                    }
                }

            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;

        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag)
        {
            var Spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (flag)
            {
                Order.Status = OrderStatus.PaymentRecevied;
            }
            else
            {
                Order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(Order);
           await _unitOfWork.CompleteAsync();
            return Order;
        }
    }
}
