using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{

	public class ProductController : APIbaseController
	{
		private readonly IGenericRepositories<Product, int> _productrepo;
		private readonly IGenericRepositories<Brand, int> _brandsRepo;
		private readonly IGenericRepositories<Category, int> _categoriesRepo;

		public ProductController(IGenericRepositories<Product,int> Productrepo, IGenericRepositories<Brand,int> brandsRepo , IGenericRepositories<Category, int> categoriesRepo)
		{
			_productrepo = Productrepo;
			_brandsRepo = brandsRepo;
			_categoriesRepo = categoriesRepo;
		}


		//ActionResult<T> and IActionResult are used as return types for controller actions
		//<ActionResult<IEnumerable<Product>> // Returning a data type with possible status codes
		//IActionResult //Returning multiple response types (Ok(), NotFound(), etc.)

		//Get All
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var Spec = new ProductWithBrandAndCategorySpec();
			var products = await _productrepo.GetAllWithSpecASync(Spec);
			return Ok(products);

		}

		//Get By Id
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			var spec = new ProductWithBrandAndCategorySpec(id);
			var product = await _productrepo.GetByIdAWithSpecAsync(spec);
			return Ok(product);
		}
		//Add


		//Update


		//Delete
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpec(id);
			await _productrepo.DeleteWithSpecAsync(spec);
			return NoContent();
		}



	}

}
