using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.DTOs;
using System.Numerics;
using System;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgroMind.GP.APIs.Controllers
{

    [Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "AgriculturalExpert")]
	public class CropController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;
		private readonly UserManager<AppUser> _userManager;

		public CropController(IServiceManager serviceManager, UserManager<AppUser> userManager)
		{
			_serviceManager = serviceManager;
			_userManager = userManager;
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
				return Ok(crop);
			
		}
		//Retrieves a single Crop plan by ID  (its creator and plan type)
		[HttpGet("GetPlanInfo/{cropId}")]
		//[Authorize] // Any authenticated user can see plan info
		public async Task<ActionResult<PlanInfoDto>> GetPlanInfo(int cropId)
		{
			
				var planInfo = await _serviceManager.CropService.GetPlanAndCreatorInfoAsync(cropId);
				return Ok(planInfo);
			
		}

		//Expert Templates and Farmer Plans) with creator details and plan types

		[HttpGet("GetAllPlansWithCreatorInfo")]
		//[Authorize(Roles = "SystemAdministrator,AgriculturalExpert")] //  Admin or Expert can see all plans ??
		public async Task<ActionResult<IReadOnlyList<PlanInfoDto>>> GetAllPlansWithCreatorInfo()
		{
			
				var allPlans = await _serviceManager.CropService.GetAllPlansWithCreatorInfoAsync();
				return Ok(allPlans);
			
		}

		//Retrieves all Crop plans belonging to the authenticated farmer.

		[HttpGet("GetMyPlans")]
		//[Authorize(Roles = "Farmer")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetMyPlans(string farmerUserId)
		{
			
			
				var myPlans = await _serviceManager.CropService.GetMyPlansAsync(farmerUserId);
				return Ok(myPlans);
			
			
		}
		[HttpPost("GetRecommendedCrops")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetRecommendedCrops([FromBody] RecommendRequestDTO request)
		{
			
				var crops = await _serviceManager.CropService.GetRecommendedCropsAsync(request);
				return Ok(crops);
			
		}

		[HttpGet("DeletedCrops")]
		//[Authorize(Roles = "SystemAdministratot")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetDeletedCrops()
		{
			var deletedCrops = await _serviceManager.CropService.GetAllDeletedCropsAsync();
			return Ok(deletedCrops);
		}

		//Adds a new Crop plan definition
		//(used by farmers or Expert

		[HttpPost("AddCrop")]
		[Authorize] // All users can create a plan, but role determines type
		public async Task<ActionResult<CropDto>> AddCrop([FromBody] CropDefinitionDto cropDto)
		{
			
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) 
				return Unauthorized("User ID not found.");

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return Unauthorized("User not found.");

			var roles = await _userManager.GetRolesAsync(user);
			var creatorRole = roles.FirstOrDefault(); // Assumes user has one primary role

			
			var createdCrop = await _serviceManager.CropService.AddCropAsync(cropDto, userId, creatorRole);
			return Ok(createdCrop);
			
		}


		


		//Updates an existing Crop plan definition, including its nested Stages and Steps

		[HttpPut("UpdateCrop/{id}")]
		//[Authorize] // Experts and Farmers can update their plans
		public async Task<IActionResult> UpdateCrop(int id, [FromBody] CropDefinitionDto cropDto)
		{
			if (id != cropDto.Id) return BadRequest("Crop ID mismatch.");
		
			var modifierUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(modifierUserId)) return Unauthorized("User ID not found.");

		
				await _serviceManager.CropService.UpdateCrops(cropDto, modifierUserId);
				return NoContent(); // 204 No Content for successful update
			
		}

		// Adopts a recommended Expert Template plan, creating a new Farmer Plan instance

		[HttpPost("AdoptRecommendedCrop/{recommendedCropId}")]
		//[Authorize(Roles = "Farmer")]
		public async Task<ActionResult<CropDto>> AdoptRecommendedCrop(int recommendedCropId)
		{
			var farmerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerUserId)) return Unauthorized("Farmer ID not found.");

			
				var adoptedCrop = await _serviceManager.CropService.AdoptRecommendedCropAsync(recommendedCropId, farmerUserId);
				return Ok (adoptedCrop);
		}


		//Updates actual cost and date information for a Crop plan, its stages, and its steps

		[HttpPut("{cropId}/Actuals")]
		//[Authorize(Roles = "Farmer,AgriculturalExpert")]
		public async Task<IActionResult> UpdateActualsForCrop(int cropId, [FromBody] CropDto cropDtoWithActuals)
		{
			if (cropId != cropDtoWithActuals.Id) return BadRequest("Crop ID mismatch.");

			var modifierUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(modifierUserId)) return Unauthorized("User ID not found.");

			
				await _serviceManager.CropService.UpdateActualsForCropAsync(cropId, cropDtoWithActuals, modifierUserId);
				return NoContent();
			
		}

		// Delete Crop
		[HttpDelete("DeleteCrop/{id}")]
		public async Task<IActionResult> DeleteCrop(int id)
		{
		
			
				await _serviceManager.CropService.DeleteCrop(id); // Pass minimal DTO
				return NoContent();
			
		}
		
	

		
	}

	//POST	Create	Body (JSON)	[FromBody]

	//PUT	Update	Route (id) + Body	[FromRoute] + [FromBody]

	//DELETE	Delete	Route (id)	[FromRoute]

	//GET	Read / Fetch	Route or Query String	[FromRoute] or [FromQuery]

}
