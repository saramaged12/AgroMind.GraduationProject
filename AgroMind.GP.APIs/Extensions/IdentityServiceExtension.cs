
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


			// To get ILogger inside this static method, we need to get the logger service
			//var serviceProvider = services.BuildServiceProvider();
			//var logger = serviceProvider.GetRequiredService<ILogger<IdentityServiceExtension>>(); // Get a logger

			// Configure Authentication
			services.AddAuthentication(options =>
			{
				Console.WriteLine("DEBUG: START - Configuring AuthenticationOptions (Default Schemes)");
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				Console.WriteLine($"DEBUG: DefaultAuthenticateScheme set to: {options.DefaultAuthenticateScheme}");
				Console.WriteLine($"DEBUG: DefaultChallengeScheme set to: {options.DefaultChallengeScheme}");
				Console.WriteLine($"DEBUG: DefaultScheme set to: {options.DefaultScheme}");
				Console.WriteLine("DEBUG: END - Configuring AuthenticationOptions (Default Schemes)");
			})
			.AddJwtBearer(options =>
			{
				Console.WriteLine("DEBUG: START - Configuring JwtBearerOptions");

				var jwtKey = configuration["JWT:key"];
				if (string.IsNullOrEmpty(jwtKey))
				{
					var errorMessage = "ERROR: JWT Key is null or empty in configuration. Please check appsettings.json (JWT:key).";
					Console.WriteLine(errorMessage);
					// In a real app, you might want to throw or handle this more gracefully depending on startup requirements.
					// For now, we'll log and proceed, but token validation will fail.
					// throw new ArgumentNullException(nameof(jwtKey), errorMessage);
				}
				else
				{
					Console.WriteLine($"DEBUG: JWT Key successfully retrieved. Length: {jwtKey.Length}. First 5 chars (if exists): '{jwtKey.Substring(0, Math.Min(jwtKey.Length, 5))}'");
				}

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "DEFAULT_KEY_IF_NULL_FOR_SAFETY_BUT_SHOULD_NOT_BE_NULL")), // Provide a default or handle null
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
				Console.WriteLine("DEBUG: TokenValidationParameters configured.");
				Console.WriteLine($"DEBUG: ValidIssuer: {configuration["JWT:ValidIssuer"]}, ValidAudience: {configuration["JWT:ValidAudience"]}");


				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						Console.WriteLine($"JWT Event: OnAuthenticationFailed. Timestamp: {DateTime.UtcNow}");
						Console.WriteLine($"Exception: {context.Exception?.GetType().Name} - {context.Exception?.Message}");
						// Log the full exception if needed, but be careful with sensitive data in logs
						// Console.WriteLine($"Full Exception: {context.Exception?.ToString()}");
						return Task.CompletedTask;
					},
					OnTokenValidated = context =>
					{
						Console.WriteLine($"JWT Event: OnTokenValidated. Timestamp: {DateTime.UtcNow}");
						Console.WriteLine($"Principal Identity Name: {context.Principal?.Identity?.Name ?? "N/A"}");
						Console.WriteLine($"Authentication Scheme: {context.Principal?.Identity?.AuthenticationType ?? "N/A"}");
						// You can inspect claims here:
						// foreach (var claim in context.Principal.Claims)
						// {
						//    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
						// }
						return Task.CompletedTask;
					},
					OnChallenge = context =>
					{
						Console.WriteLine($"JWT Event: OnChallenge. Timestamp: {DateTime.UtcNow}");
						// This event is triggered when authentication is required but has failed or is not provided.
						// Default behavior is to return a 401 Unauthorized.
						// You can customize the response here if needed.
						// context.HandleResponse(); // Call this if you want to skip the default challenge logic and provide your own response.
						// context.Response.StatusCode = 401;
						// context.Response.ContentType = "application/json";
						// var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Custom OnChallenge: You are not authorized." });
						// return context.Response.WriteAsync(result);
						Console.WriteLine("DEBUG: OnChallenge - Default behavior will proceed (return 401).");
						return Task.CompletedTask;
					},
					OnForbidden = context =>
					{
						Console.WriteLine($"JWT Event: OnForbidden. Timestamp: {DateTime.UtcNow}");
						// This event is triggered when authentication was successful but authorization failed.
						// Default behavior is to return a 403 Forbidden.
						// You can customize the response here.
						Console.WriteLine("DEBUG: OnForbidden - Default behavior will proceed (return 403).");
						return Task.CompletedTask;
					},
					OnMessageReceived = context =>
					{
						Console.WriteLine($"JWT Event: OnMessageReceived. Timestamp: {DateTime.UtcNow}");
						// This event is triggered when a message is received, allowing you to extract the token from a non-standard location if necessary.
						// For example, from a query string for SignalR.
						// var accessToken = context.Request.Query["access_token"];
						// var path = context.HttpContext.Request.Path;
						// if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hubs"))) // Example for SignalR
						// {
						//    Console.WriteLine("DEBUG: Token received in query string for hub.");
						//    context.Token = accessToken;
						// }
						return Task.CompletedTask;
					}
				};
				Console.WriteLine("DEBUG: JwtBearerEvents configured.");
				Console.WriteLine("DEBUG: END - Configuring JwtBearerOptions");
			});

			Console.WriteLine("DEBUG: Authentication and JwtBearer configuration calls have been made.");

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

			Console.WriteLine("DEBUG: AddIdentityServices method finished.");
			return services;



			//// Configure JWT Bearer Authentication
			//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			//	.AddJwtBearer(options =>
			//	{
			//		var jwtKey = configuration["JWT:key"];
			//		if (string.IsNullOrEmpty(jwtKey))
			//		{
			//			var errorMessage = "ERROR: JWT Key is null or empty in configuration. Please check appsettings.json (JWT:key).";
			//			Console.WriteLine(errorMessage);
			//			// In a real app, you might want to throw or handle this more gracefully depending on startup requirements.
			//			// For now, we'll log and proceed, but token validation will fail.
			//			// throw new ArgumentNullException(nameof(jwtKey), errorMessage);
			//		}
			//		else
			//		{
			//			Console.WriteLine($"DEBUG: JWT Key successfully retrieved. Length: {jwtKey.Length}. First 5 chars (if exists): '{jwtKey.Substring(0, Math.Min(jwtKey.Length, 5))}'");
			//		}

			//		options.RequireHttpsMetadata = false; //True in Case Production
			//		options.SaveToken = true; // Saves the token in HttpContext.Features (useful for debugging)
			//		options.TokenValidationParameters = new TokenValidationParameters
			//		{
			//			ValidateIssuer = true,
			//			ValidateAudience = true,
			//			ValidateLifetime = true,
			//			ValidateIssuerSigningKey = true,
			//			ValidIssuer = configuration["JWT:ValidIssuer"],
			//			ValidAudience = configuration["JWT:ValidAudience"],
			//			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((jwtKey ?? "DEFAULT_KEY_IF_NULL_FOR_SAFETY_BUT_SHOULD_NOT_BE_NULL"))),// Provide a default or handle null
			//			ClockSkew = TimeSpan.Zero // ClockSkew to allow for small clock differences between servers
			//		};

				

			//	});


			//return services;
		}
	}
}

