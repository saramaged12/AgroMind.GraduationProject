using AgroMind.GP.Core.Exceptions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace AgroMind.GP.APIs.CustomMiddleWares
{
	public class CustomExceptionHandlerMiddleWare
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

		public CustomExceptionHandlerMiddleWare(RequestDelegate Next,ILogger<CustomExceptionHandlerMiddleWare> logger )
		{
			_next = Next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
		

			try
			{
				await _next.Invoke(httpContext);
				await HandleNotFoundEndPointAsync(httpContext);
				await HandleUnauthorized(httpContext);

			}
			catch (Exception ex)
			{

				_logger.LogError(ex, "Something Went Wrong");
				await HandleException(httpContext, ex);
				
			}
		}

		private static async Task HandleException(HttpContext httpContext, Exception ex)
		{
			//1-Set Status Code for Response

			//httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error
			//httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; // the header of Request it self

			httpContext.Response.StatusCode = ex switch
			{
				
				NotFoundException => StatusCodes.Status404NotFound,
				
				_ => StatusCodes.Status500InternalServerError // Default to Internal Server Error
			};

			//2-Set Content-Type for Response

			//httpContext.Response.ContentType = "application/json"; not be needed if Use "WriteAsJsonAsync"

			//3-Response Object (StatusCode , ErrorMessage)
			var Response = new ErrorToReturn()
			{
				StatusCode = httpContext.Response.StatusCode, //will show in the response body
				ErrorMessage = ex.Message // You can customize the error message as needed
			};


			//4-Return Object As Json

			//var ResponseToReturn = JsonSerializer.Serialize(Response);
			//await httpContext.Response.WriteAsync(ResponseToReturn);

			await httpContext.Response.WriteAsJsonAsync(Response);//convert the object to JSON and write it to the response body
		}

		private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
		{
			if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
			{
				//Handle Not Found EndPoint
				var Response = new ErrorToReturn()
				{
					StatusCode = StatusCodes.Status404NotFound,
					ErrorMessage = $"The requested resource {httpContext.Request.Path} is not found."
				};

				await httpContext.Response.WriteAsJsonAsync(Response); 
			}
		}

		private static async Task HandleUnauthorized(HttpContext httpContext)
		{
			if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
			{
				//Handle Not Found EndPoint
				var Response = new ErrorToReturn()
				{
					StatusCode = StatusCodes.Status401Unauthorized,
					ErrorMessage = "You are not authorized to access this resource."
				};

				await httpContext.Response.WriteAsJsonAsync(Response);
			}
		}
	}
}
