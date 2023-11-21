using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.core.Entities.Identity;
using Talabat.core.Services;
using Talabat.services;

namespace Talabat.APIS.Controllers
{
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager ,
            SignInManager<AppUser> signInManager,
            TokenService tokenService,
            IMapper mapper
            
            )
        {
            _userManager = userManager;
           _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400 , "This EMail is Already Exists"));




            var User = new AppUser()
            {
                DisplayName= model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber= model.PhoneNumber,
            };
           var Result = await _userManager.CreateAsync(User , model.Password);
            if(Result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }
        // Login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));

           var Result = await _signInManager.CheckPasswordSignInAsync(User , model.Password , false);

            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));


            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });

        }

        // Get Current User

        [Authorize]
        [HttpGet("GetCurrentUser")]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);

        }
        [Authorize]
        [HttpGet("Address")]

        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserByAddressAsync(User);

            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(MappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var Address = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            Address.Id = user.Address.Id;
            user.Address = Address;
            var Result = await _userManager.UpdateAsync(user);
            if(!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);


        }


        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
       
          return await _userManager.FindByEmailAsync(Email) is not null;
        }
    }
}
