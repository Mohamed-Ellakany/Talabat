using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.DTOs;
using Talabat.Errors;

namespace Talabat.Controllers
{

    public class BasketsController : APIsBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        //get or recreate 


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Basket =await _basketRepository.GetBasketAsync(BasketId);

            ////if (Basket is null) return new CustomerBasket(BasketId); 


            ////return Ok(Basket);

            return Basket is null ? new CustomerBasket(BasketId) :Basket ;
        }


        //update or create

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket)
        {

            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);

            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);

            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));

            return Ok(CreatedOrUpdatedBasket);

        }


        //delete 
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);

        }
    }
}
