using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.core;
using Talabat.core.Entities;
using Talabat.core.Repositories;
using Talabat.core.Specifications;

namespace Talabat.APIS.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public ProductsController(IMapper mapper , IUnitOfWork unitOfWork) {
           
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //Get All Products
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _unitOfWork.Repository<Products>().GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Products> , IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
                var Count = await _unitOfWork.Repository<Products>().GetCountWithSpecAsync(CountSpec);

            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex , Params.PageSize , MappedProducts , Count));    
        }
        // Get Product By Id
        //BaseUrl/api/products/1 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Products>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _unitOfWork.Repository<Products>().GetEntityWithSpecAsync(Spec);
            if(Product is null) return NotFound(new ApiResponse(404));
            var MappedProducts = _mapper.Map<Products, ProductToReturnDto>(Product);
            return Ok(MappedProducts);
        }

        //Get All Types
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }
        //Get All Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
    }
}
