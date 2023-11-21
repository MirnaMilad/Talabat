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
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        //private readonly IGenericRepository<Products> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethod;
        //private readonly IGenericRepository<Order> _orderRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;

        public OrderService(IBasketRepository basketRepository ,
            //IGenericRepository<Products> ProductRepo ,
            //IGenericRepository<DeliveryMethod> DeliveryMethod ,
            //IGenericRepository<Order> OrderRepo,
            IUnitOfWork unitOfWork,
            IPaymentServices paymentServices
            )
        {
            _basketRepository = basketRepository;
            //_productRepo = ProductRepo;
            //_deliveryMethod = DeliveryMethod;
            //_orderRepo = OrderRepo;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Products>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrder(Product.Name , Product.Id , Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered , Product.Price , item.Quantity);
                    OrderItems.Add(OrderItem);

                }
            }
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(ExOrder is not null) {
            _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            }

            var Order = new Order(buyerEmail , ShippingAddress , DeliveryMethod , OrderItems , SubTotal , Basket.PaymentIntentId);
           await _unitOfWork.Repository<Order>().Add(Order);
           var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0)
            {
                return null;
            }
            return Order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var Spec = new OrderSpec(buyerEmail, orderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpec(buyerEmail);
            var Orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
    }
}
