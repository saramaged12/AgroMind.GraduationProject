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
			}
			catch (Exception ex)
			{

				_logger.LogError(ex,"Something Went Wrong");

				//1-Set Status Code for Response

				//httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error


				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; // the header of Request it self
				
				//2-Set Content-Type for Response

				//httpContext.Response.ContentType = "application/json"; not be needed if Use "WriteAsJsonAsync"
				
				//3-Response Object (StatusCode , ErrorMessage)
				var Response= new ErrorToReturn
				{
					StatusCode = StatusCodes.Status500InternalServerError, //will show in the response body
					ErrorMessage = ex.Message // You can customize the error message as needed
				};
				{

				}

				//4-Return Object As Json

				//var ResponseToReturn = JsonSerializer.Serialize(Response);
				//await httpContext.Response.WriteAsync(ResponseToReturn);
				
				await httpContext.Response.WriteAsJsonAsync(Response);//convert the object to JSON and write it to the response body
			}
		}
	}
}
