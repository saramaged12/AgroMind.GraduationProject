
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Contexts;
using AgroMind.GP.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AgroMind.GP.APIs.Extensions
{
    public static class IdentityServiceExtension
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<ITokenService, TokenService>();

			// Configure Identity (User and Role management)
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{

				options.Password.RequireNonAlphanumeric = true; //@ # $
				options.Password.RequireDigit = true;  //123
				options.Password.RequireLowercase = true; //abc
				options.Password.RequireUppercase = true; //ABC
				options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
			})
			.AddEntityFrameworkStores<AgroMindContext>()  // Link Identity to  DbContext

			.AddDefaultTokenProviders(); // Provides default token providers for password reset, email confirmation

			// Configure JWT Bearer Authentication
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false; //True in Case Production
					options.SaveToken = true; // Saves the token in HttpContext.Features (useful for debugging)
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidAudience = configuration["JWT:ValidAudience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"])),
					    ClockSkew = TimeSpan.Zero // ClockSkew to allow for small clock differences between servers
					};
				});


			return services;
		}
	}
}

