using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Services;

namespace Talabat.Controllers
{
    public class OrdersController : APIsBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(IReadOnlyList<Order>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

            var Order =await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId , orderDto.DeliveryMethodId , mappedAddress);


            if (Order is null) return BadRequest(new ApiResponse(400, "there is a problem with your order"));


            return Ok(Order);
        }


        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForSpecificUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders =await _orderService.GetOrderForSpecificUserAsync(BuyerEmail);
            if (orders is null) return NotFound(new ApiResponse(404, "this no order for this user"));

            var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(MappedOrder);


        }


        [ProducesResponseType(typeof(OrderToReturnDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int Id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUser(BuyerEmail , Id);

            if (Order is null) return NotFound(new ApiResponse(404, $"there is no order with Id = {Id}"));

            var MappedOrder = _mapper.Map<Order , OrderToReturnDto>(Order);

            return Ok(MappedOrder);
        
        }

        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
          var DeliveryMethods =  await _orderService.GetAllDeliveryMethods();
        
            return Ok(DeliveryMethods);
        }



    }
}