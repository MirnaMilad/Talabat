using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.core;
using Talabat.core.Entities.OrderAggrigation;
using Talabat.services;

namespace Talabat.APIS.Controllers
{
    
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] // POst=>baseUrl/api/Orders
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email); 
            var MappedAddress = _mapper.Map<AddressDto , Address>(orderDto.shipToAddress);
            var Order = _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId , orderDto.DeliveryMethodId , MappedAddress);
                if (Order is null) return BadRequest(new ApiResponse(400 , "There is a problem with your Order"));
                //var MappedOrder = _mapper.Map<Order,OrderToReturnDto>
                return Ok(Order);
        }

        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There is no orders for this user"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, id);
            if (Order is null) return NotFound(new ApiResponse(404, $"There is no order with Id = {id} for this user"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(Order);
            return Ok(MappedOrder);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(DeliveryMethods);
        }

    }
}
