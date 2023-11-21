using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.core.Entities;
using Talabat.core.Repositories;

namespace Talabat.APIS.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        // Get Or create
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket (string BasketId)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            return Basket is null ? new CustomerBasket(BasketId) : Basket;
        }




        // Update Or Create new basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket (CustomerBasketDto Basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);
           var CreatedOrUPdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUPdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUPdatedBasket);
        }


        //Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket (string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
