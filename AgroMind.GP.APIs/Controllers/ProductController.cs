using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
			public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts()
			{
				var products = await _serviceManager.ProductService.GetAllProductsAsync();
				return Ok(products);
			}

			// Get Product By Id
			[HttpGet("GetProductById/{id}")]
			public async Task<ActionResult<ProductDTO>> GetProductById(int id)
			{
				var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
				if (product == null)
					return NotFound($"Product with ID {id} not found.");

				return Ok(product);
			}

		// Add Product
		[HttpPost("AddProduct")]
		public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO productDto)
		{
			if (productDto == null)
				return BadRequest("Product data is required.");

			// Call the service to add the product and return the created product
			var createdProduct = await _serviceManager.ProductService.AddAsync(productDto);

			if (createdProduct == null)
				return BadRequest("Failed to create the product.");

			return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);

		}

		// Update Product
		[HttpPut("UpdateProduct/{id}")]
			public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
			{
				if (id != productDto.Id)
					return BadRequest("Product ID mismatch.");

				var existingProduct = await _serviceManager.ProductService.GetProductByIdAsync(id);
				if (existingProduct == null)
					return NotFound($"Product with ID {id} not found.");

				await _serviceManager.ProductService.UpdateProducs(productDto);
				return NoContent();
			}

			// Delete Product
			[HttpDelete("DeleteProduct/{id}")]
			public async Task<IActionResult> DeleteProduct(int id)
			{
				var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
				if (product == null)
					return NotFound($"Product with ID {id} not found.");

				await _serviceManager.ProductService.DeleteProducts(new ProductDTO { Id = id });
				return NoContent();
			}
		}
	}