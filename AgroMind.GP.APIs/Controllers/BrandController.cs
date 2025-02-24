using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	
	public class BrandController : APIbaseController
	{
		private readonly IGenericRepositories<Brand, int> _brandsRepo;

		public BrandController(IGenericRepositories<Brand, int> brandsRepo)
		{
			_brandsRepo = brandsRepo;
		}

		//Get All Brands
		[HttpGet("Brands")]
		public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
		{
			var brands = await _brandsRepo.GetAllAsync();
			return Ok(brands);
		}

		//Add


		//Delete

		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteProduct(Brand brand)
		//{
			
		//	await _brandsRepo.Delete(brand);
		//	return NoContent();
		//}

		//Update


	}
}
