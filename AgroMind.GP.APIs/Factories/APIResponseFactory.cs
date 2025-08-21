using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace AgroMind.GP.APIs.Factories
{
	public  static class APIResponseFactory
	{
		public static IActionResult GenerateApiValidationErrorResponse(ActionContext context)
		{
			var Errors = context.ModelState.Where(M => M.Value.Errors.Any())
						.Select(M => new ValidationError()
						{
							Field = M.Key,
							Errors = M.Value.Errors.Select(e => e.ErrorMessage).ToArray()
						}).ToArray();
			var Response = new ValidationErrorToReturn()
			{
				ValidationErrors = Errors
			};
			return new BadRequestObjectResult(Response);

		}
	}
}
