using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Repository.Data.Contexts;
using AgroMind.GP.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AgroMind.GP.APIs.Extensions.DependencyInjection
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
           

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

			// Configure DataProtectionTokenProviderOptions (for password reset tokens, etc.)
			services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromHours(2); 
			});


			// To get ILogger inside this static method, we need to get the logger service
			//var serviceProvider = services.BuildServiceProvider();
			//var logger = serviceProvider.GetRequiredService<ILogger<IdentityServiceExtension>>(); // Get a logger

			// Configure Authentication
			services.AddAuthentication(options =>
            {
               
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                
            })
            .AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"] ?? throw new InvalidOperationException("JWT Key not configured."))), // Provide a default or handle null
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"] ?? throw new InvalidOperationException("JWT Issuer not configured."),
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

            });
                // Configure Authorization
                services.AddAuthorization(options =>
                {
                    Console.WriteLine("DEBUG: START - Configuring Authorization policies.");
                    options.AddPolicy("AdminRole", policy => policy.RequireRole("Admin", "SuperAdmin"));
                    options.AddPolicy("FarmerRole", policy => policy.RequireRole("Farmer")); // Ensure "Farmer" matches the role string in your JWT
                    options.AddPolicy("AgriculturalExpertRole", policy => policy.RequireRole("AgriculturalExpert"));
                    options.AddPolicy("SupplierRole", policy => policy.RequireRole("Supplier"));
                    Console.WriteLine("DEBUG: END - Configuring Authorization policies.");
                });
            
            return services;




        }
    }
}

