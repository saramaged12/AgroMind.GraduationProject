using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : APIbaseController
		{
			private readonly IServiceManager _serviceManager;

			public ProductController(IServiceManager serviceManager)
			{
				_serviceManager = serviceManager;
			}

			// Get All Products
			[HttpGet("GetProducts")]
			public async Task<ActionResult<PaginatedResultDTO<ProductDTO>>> GetProducts([FromQuery]ProductQueryParams QueryParams)
			{
				var products = await _serviceManager.ProductService.GetAllProductsAsync(QueryParams);
				return Ok(products);
			}

			// Get Product By Id
			[HttpGet("GetProductById/{id}")]
			public async Task<ActionResult<ProductDTO>> GetProductById(int id)
			{
				var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
				return Ok(product);
			}

		    
		    [HttpPost("AddProduct")]
		    public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO productDto)
		    {
			   
			    var createdProduct = await _serviceManager.ProductService.AddAsync(productDto);


		       	return Ok(createdProduct);
		    }

		    
		    [HttpPut("UpdateProduct/{id}")]
			public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
			{
				if (id != productDto.Id)
					return BadRequest("Product ID mismatch.");

				await _serviceManager.ProductService.UpdateProducs(productDto);
				return NoContent();
			}

			// Delete Product
			[HttpDelete("DeleteProduct/{id}")]
			public async Task<IActionResult> DeleteProduct(int id)
			{
				
				await _serviceManager.ProductService.DeleteProducts(id);
				return NoContent();
			}

	    	//endpoint to view deleted products (admin only)
		    [HttpGet("DeletedProducts")]
		    //[Authorize(Roles = "SystemAdministratot")]
		    public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetDeletedProducts()
		    {
			var deletedProducts = await _serviceManager.ProductService.GetAllDeletedProductsAsync();
			return Ok(deletedProducts);
		    }

	}
	}