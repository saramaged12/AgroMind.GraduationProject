using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{

	public class ProductController : APIbaseController
	{
		private readonly IGenericRepositories<Product, int> _productrepo;

		public ProductController(IGenericRepositories<Product,int> Productrepo)
		{
			_productrepo = Productrepo;
		}


		//ActionResult<T> and IActionResult are used as return types for controller actions
		//<ActionResult<IEnumerable<Product>> // Returning a data type with possible status codes
		//IActionResult //Returning multiple response types (Ok(), NotFound(), etc.)

		//Get All
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts() 
		{
		  var products= await _productrepo.GetAllAsync();
			return Ok(products);
		
		}

		//Get By Id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
            var product = await _productrepo.GetByIdAsync(id);
			return Ok(product);
		}
	}
}
