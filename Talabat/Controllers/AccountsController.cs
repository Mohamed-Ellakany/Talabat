using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Extensions;

namespace Talabat.Controllers
{

    public class AccountsController : APIsBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _token;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices token, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
            _mapper = mapper;
        }


        //register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExist(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email Is Already In Use"));
            }

            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber

            };
            var result = await _userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));


            var ReturnedUser = new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _token.CreateTokenAsync(User, _userManager)
            };

            return Ok(ReturnedUser);
        }



        //Login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);

            if (User is null) return Unauthorized(new ApiResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);

            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName
                ,
                Email = User.Email
                ,
                Token = await _token.CreateTokenAsync(User, _userManager)
            });



        }

        [Authorize]
        [HttpGet("GetCurrentUser")]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(Email);

            var ReturnedObject = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email
                ,
                Token = await _token.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedObject);
        }



        [Authorize]
        [HttpGet("Address")]

        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
        }



        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(Email);

            //var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);

            var user = await _userManager.FindUserWithAddressAsync(User);

            var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);

            MappedAddress.Id = user.Address.Id;

            user.Address = MappedAddress;
            var result = await _userManager.UpdateAsync(user);
             
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);


        }



        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string Email)
        {

           return await _userManager.FindByEmailAsync(Email) is not null;

            //if (User is null) return false;
            //else return true;
        } 










    
    }





}
