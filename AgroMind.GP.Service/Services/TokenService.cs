using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AgroMind.GP.Service.Services
{
    public class TokenService :ITokenService
	{


		//Header -> algorithm , token
		//PayLoad
		//Keys

		private readonly IConfiguration _configuration;
		private readonly ILogger<TokenService> _logger;

		public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
		{
			_configuration = configuration;
			_logger = logger;
		}
		public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
		{
			//PayLoad
			//1.Private Claims [Name]

			var roles=await userManager.GetRolesAsync(user);

			var AutClaim=new List<Claim>() //Claim : Propertoes For User ..Name , Email ,...
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(ClaimTypes.Name,user.UserName),
			
			};
			var userRoles= await userManager.GetRolesAsync(user); //Take User and Return El Role
			foreach(var role in userRoles)
			{
				AutClaim.Add(new Claim(ClaimTypes.Role, role));
			}

			//Key
			//var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
			//var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));


			var jwtKey = _configuration["JWT:key"];
			if (string.IsNullOrEmpty(jwtKey))
			{
				_logger.LogError("JWT:key is missing or empty in the configuration!"); // Log this error
				throw new InvalidOperationException("JWT:key is missing in the configuration.");
			}

			//  log the key being used 
			_logger.LogInformation($"JWT Key used for token creation: '{jwtKey}' (Length: {jwtKey.Length})");

			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));


			//Register Claims:
			//iss(issuer):will create token > BaseUrl
			//aud(audience):static , will create بيه any token will create by me
			//exp(expirationTime)

			//Token

			var Token = new JwtSecurityToken(

				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				claims:AutClaim,
				signingCredentials: new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
			);

			//Generate Token
			return new JwtSecurityTokenHandler().WriteToken(Token);	
		}
	}
}
