using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : APIbaseController
		{
			private readonly IServiceManager _serviceManager;

			public CategoryController(IServiceManager serviceManager)
			{
				_serviceManager = serviceManager;
			}

			// Get All Categories
			[HttpGet("Categories")]
			public async Task<ActionResult<IReadOnlyList<CategoryDTO>>> GetCategories()
			{
				var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
				return Ok(categories);
			}

			// Get Category By Id
			[HttpGet("GetCategoryById/{id}")]
			public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
			{
				var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
				if (category == null)
					return NotFound($"Category with ID {id} not found.");

				return Ok(category);
			}

			// Add Category
			[HttpPost("AddCategory")]
			public async Task<ActionResult<CategoryDTO>> AddCategory([FromBody] CategoryDTO categoryDto)
			{
				if (categoryDto == null)
					return BadRequest("Category data is required.");


				var CategoryCreated=await _serviceManager.CategoryService.AddCategoryAsync(categoryDto);

			     if (CategoryCreated == null)
			    	return BadRequest("Failed to create the Category.");

			        return CreatedAtAction(nameof(GetCategoryById), new { id = CategoryCreated.Id }, CategoryCreated);
			}

			// Update Category
			[HttpPut("UpdateCategoryById/{id}")]
			public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
			{
				if (id != categoryDto.Id)
					return BadRequest("Category ID mismatch.");

				var existingCategory = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
				if (existingCategory == null)
					return NotFound($"Category with ID {id} not found.");

				await _serviceManager.CategoryService.UpdateCategories(categoryDto);
				return NoContent();
			}

			// Delete Category
			[HttpDelete("DeleteCategoryById/{id}")]
			public async Task<IActionResult> DeleteCategory(int id)
			{
				var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
				if (category == null)
					return NotFound($"Category with ID {id} not found.");

				await _serviceManager.CategoryService.DeleteCategories(new CategoryDTO { Id = id });
				return NoContent();
			}
		}
	}

