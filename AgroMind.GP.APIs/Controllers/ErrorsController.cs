using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System.Net.Http;

namespace AgroMind.GP.APIs.Controllers
{
	
	[ApiController]
	[Route("errors/{code}")]
	[ApiExplorerSettings(IgnoreApi = true)] // This hides the controller from Swagger UI
	public class ErrorsController : ControllerBase
	{
		
		public  IActionResult error(int code)
		{
			
			// We create a standard response object and return it.
			// The status code of the HTTP response will be set by ObjectResult.
			return new ObjectResult(new ErrorToReturn
			{
				StatusCode = code,
				ErrorMessage = GetDefaultMessageForStatusCode(code)
			})
			{
				StatusCode = code
			};
		}

		private string GetDefaultMessageForStatusCode(int statusCode) => statusCode switch
		{
			400 => "A bad request was made.",
			401 => "You are not authenticated to access this resource.",
			403 => "You do not have permission to perform this action.",
			404 => "The resource you requested could not be found.",
			405 => "The HTTP method is not allowed for this endpoint.",
			_ => "An unexpected error occurred." // Fallback for other codes
		};
	}
}

