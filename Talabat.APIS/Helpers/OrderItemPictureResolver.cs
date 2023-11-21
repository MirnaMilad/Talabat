//using AutoMapper.Execution;
using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.APIS.Helpers
{
    public class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
