using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.core.Entities;

namespace Talabat.APIS.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Products, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) {
        _configuration = configuration;
        }

        public string Resolve(Products source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
