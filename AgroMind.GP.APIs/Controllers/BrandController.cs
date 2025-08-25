using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BrandController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;

		public BrandController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

	
			// Get All Brands
			[HttpGet("GetAllBrands")]
			public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetBrands()
			{
				var brands = await _serviceManager.BrandService.GetAllBrandsAsync();
				return Ok(brands);
			}

			// Get Brand By Id
			[HttpGet("GetBrandById/{id}")]
			public async Task<ActionResult<BrandDTO>> GetBrandById(int id)
			{
				var brand = await _serviceManager.BrandService.GetBrandsByIdAsync(id);
				
				return Ok(brand);
			}

			// Add Brand
			[HttpPost("AddBrand")]
			public async Task<ActionResult<BrandDTO>> AddBrand([FromBody] BrandDTO brandDto)
			{
				var Brand= await _serviceManager.BrandService.AddBrandAsync(brandDto);
				return CreatedAtAction(nameof(GetBrandById), new { id = Brand.Id }, Brand);
			}

		

		   // Update Brand
		   [HttpPut("UpdateBrandById/{id}")]
           public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDTO brandDto)
		   {
			if (id != brandDto.Id)
				throw new BadRequestException("Brand ID mismatch , Brand ID in route must match Brand ID in body.");
				await _serviceManager.BrandService.UpdateBrands(brandDto);
				return NoContent();
		   }



		    // Delete Brand
		    [HttpDelete("DeleteBrand/{id}")]
			public async Task<IActionResult> DeleteBrand(int id)
			{
				await _serviceManager.BrandService.DeleteBrands(id);
				return NoContent();
			}


		   
		     [HttpGet("DeletedBrands")]
		    //[Authorize(Roles = "SystemAdministratot")]
	       	public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetDeletedBrands()
		    {
			var deletedBrands = await _serviceManager.BrandService.GetAllDeletedBrandsAsync();
			return Ok(deletedBrands);
		    }

	}
	}


 