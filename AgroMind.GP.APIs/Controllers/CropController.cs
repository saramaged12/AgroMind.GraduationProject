using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.DTOs;

namespace AgroMind.GP.APIs.Controllers
{

    [Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "AgriculturalExpert")]
	public class CropController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;

		public CropController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		// Get All Crops
		[HttpGet("GetCrops")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetCropss()
		{
			var crops = await _serviceManager.CropService.GetAllCropsAsync();
			return Ok(crops);
		}

		// Get Crop By Id
		[HttpGet("GetCropById/{id}")]
		public async Task<ActionResult<CropDto>> GetCropById(int id)
		{
			var crop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (crop == null)
				return NotFound($"Crop with ID {id} not found.");

			return Ok(crop);
		}

		// Add Crop
		[HttpPost("AddCrop")]
		public async Task<ActionResult<CropDto>> AddCrop([FromBody] CropDto cropDto)
		{
			if (cropDto == null)
				return BadRequest(" Crop data is required.");

			// Call the service to add the Crop and return the created Crop
			var createdcrop = await _serviceManager.CropService.AddCropAsync(cropDto);

			if (createdcrop == null)
				return BadRequest("Failed to create the Crop.");

			return CreatedAtAction(nameof(GetCropById), new { id = createdcrop.Id }, createdcrop);

		}

		// Update Crop
		[HttpPut("UpdateCrop/{id}")]
		public async Task<IActionResult> UpdateCrop(int id, [FromBody] CropDto cropDto)
		{
			if (id != cropDto.Id)
				return BadRequest("Crop ID mismatch.");

			var existingCrop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (existingCrop == null)
				return NotFound($"Crop with ID {id} not found.");

			await _serviceManager.CropService.UpdateCrops(cropDto);
			return NoContent();
		}

		// Delete Crop
		[HttpDelete("DeleteCrop/{id}")]
		public async Task<IActionResult> DeleteCrop(int id)
		{
			var crop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (crop == null)
				return NotFound($"Crop with ID {id} not found.");

			await _serviceManager.CropService.DeleteCrop(new CropDto { Id = id });
			return NoContent();
		}
		
		[HttpGet("DeletedCrops")]
		//[Authorize(Roles = "SystemAdministratot")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetDeletedCrops()
		{
			var deletedCrops = await _serviceManager.CropService.GetAllDeletedCropsAsync();
			return Ok(deletedCrops);
		}

		[HttpPost("GetRecommendedCrops")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetRecommendedCrops([FromBody] RecommendRequestDTO request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var crops = await _serviceManager.CropService.GetRecommendedCropsAsync(request);
				return Ok(crops);
			}
			catch (RecommendationException ex)
			{
				return NotFound(new { Message = ex.Message, Reasons = ex.Reasons });
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine($"Error in GetRecommendedCrops: {ex.Message}");
				return StatusCode(500, "An unexpected error occurred.");
			}
		}
	}

	//POST	Create	Body (JSON)	[FromBody]

	//PUT	Update	Route (id) + Body	[FromRoute] + [FromBody]

	//DELETE	Delete	Route (id)	[FromRoute]

	//GET	Read / Fetch	Route or Query String	[FromRoute] or [FromQuery]

}
