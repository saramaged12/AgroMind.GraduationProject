using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	
	public class CategoryController : APIbaseController
	{
		private readonly IGenericRepositories<Category, int> _categoriesRepo;

		public CategoryController(IGenericRepositories<Category, int> categoriesRepo)
		{
			_categoriesRepo = categoriesRepo;
		}

		//Get All Categoriess
		[HttpGet("Categories")]

		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			var categories = await _categoriesRepo.GetAllAsync();
			return Ok(categories);
		}

	}
}
