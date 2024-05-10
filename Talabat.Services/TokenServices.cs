using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser User , UserManager<AppUser> userManager)
        {
            //payload
            //1. private claims [User - Defind]

            var AuthClaims=new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , User.DisplayName),
                new Claim(ClaimTypes.Email , User.Email)

            };


            var UserRoles = await userManager.GetRolesAsync(User);
            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));

            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"]  
                , audience : _configuration["JWT:ValidAudience"] ,
                expires : DateTime.Now.AddDays(double.Parse( _configuration["JWT:DurationDays"])),
                claims:AuthClaims,
                signingCredentials:new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha256Signature)
                );
         
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
