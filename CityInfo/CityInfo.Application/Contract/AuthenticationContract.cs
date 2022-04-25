﻿using CityInfo.Application.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.Application.Contract
{
    public class AuthenticationContract : IAuthenticationContract
    {
        private readonly IConfiguration _configuration;
        public AuthenticationContract(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            //Step 1: Validate the username/ password
            var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

            if (user == null)
            {
                return null;
            }

            //Step 2: create a token
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //The claims that
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;

        }

        private CityInfoUser ValidateUserCredentials(string userName, string password)
        {
            /* we don't have a user DB or table. If you have, check the passed-through use
             * username/password against what's stored in the database
             * 
             * For demo purposaes, we assume the credentials are valid
             * 
             * return a new CityInfoUser (values would normally come from your user DB/table
             */

            return new CityInfoUser(
                1,
                userName ?? "",
                "Sean",
                "Pietersen",
                "Cape Town");
        }
    }
}
