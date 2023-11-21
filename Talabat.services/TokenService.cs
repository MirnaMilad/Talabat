﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.Identity;
using Talabat.core.Services;

namespace Talabat.services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser User , UserManager<AppUser> userManager) 
        {
            //Payload
            // 1 . Private Claims [User - Defined]
            var AuthClaim = new List<Claim>()
            {
                new Claim (ClaimTypes.GivenName , User.DisplayName),
                new Claim (ClaimTypes.Email , User.Email)
            };


            var UserRoles = await userManager.GetRolesAsync(User);
            foreach(var Role in UserRoles)
            {
                AuthClaim.Add(new Claim(ClaimTypes.Role, Role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var Token = new JwtSecurityToken(
                issuer : configuration["JWT.ValidIssuer"],
                audience: configuration["JWT.ValidAudience"],
                expires:DateTime.Now.AddDays(double.Parse(configuration["JWT.DurationInDays"])),
                claims:AuthClaim,
                signingCredentials : new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha256Signature)
                
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
