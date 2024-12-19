using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class AuthServices : IAuthSrvices
    {
        private readonly IConfiguration _configuration;

        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager)
        {


            var AuthClams = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email)
            };


            var UserRole = await userManager.GetRolesAsync(user);
            foreach (var role in UserRole)
            {
                AuthClams.Add(new Claim(ClaimTypes.Role,role));
            }


            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));


            var Token = new JwtSecurityToken
                (
                audience : _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssured"],
                expires: DateTime.Now.AddDays(double.Parse( _configuration["JWT:DurationInDays"] ?? "0")),
                claims: AuthClams,
                signingCredentials:new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature));


            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
