using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.core.Entities;
using OrderAddress = Talabat.core.Entities.OrderAggrigation.Address;
using IdentityAddress =  Talabat.core.Entities.Identity.Address;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.APIS.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Products, ProductToReturnDto>()
                .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
            

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<AddressDto , OrderAddress>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureResolver>());

            
        }
    }
}
