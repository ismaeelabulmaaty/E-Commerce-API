using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Extentions;

namespace Talabat.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthSrvices _authSrvices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , IAuthSrvices authSrvices , IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authSrvices = authSrvices;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(loginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
                return Unauthorized( new ApiResponse(401));

            var result= await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {

                DisplayName= user.DisplayName,
                Email=user.Email,
                Token= await _authSrvices.CreateTokenAsync(user , _userManager)


            });


        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if(CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorssResponse() { Errors=new string[] {"This Email Is Alredy Exist!"} });


            //creat user
            var user =new AppUser()
            {
                DisplayName= model.DisplayName,
                Email=model.Email,
                UserName = model.Email.Split("@")[0]
            };


            //create async
            var result = await _userManager.CreateAsync(user);  
            if(result.Succeeded is false) return BadRequest( new ApiResponse(400));


            //return ok(userdto)
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email=user.Email,
                Token = await _authSrvices.CreateTokenAsync(user, _userManager)
            });

        }



        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user =  await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName ?? string.Empty,
                Email=user.Email ?? string.Empty,
                Token = await _authSrvices.CreateTokenAsync(user,_userManager)
            });
        }




        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("address")]
        public async Task<ActionResult<AddressUserDto>> GetUserAddress()
        {

            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _userManager.FindUserWithAddressByEmail(User);
            return Ok(_mapper.Map<Address , AddressUserDto>(user.Address));
        }




        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("address")]
        public async Task<ActionResult<Address>> UpdateUserAddres(AddressUserDto address)
        {
            var UpdateAddress = _mapper.Map<AddressUserDto, Address>(address);
            var user = await _userManager.FindUserWithAddressByEmail(User);

            UpdateAddress.Id = user.Address.Id;

            user.Address = UpdateAddress;

            var result =await _userManager.UpdateAsync(user);
            if (!result.Succeeded ) return BadRequest(400);
            return Ok(address);
            
        }


        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
