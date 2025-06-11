using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace AgroMind.GP.APIs.Controllers
{
    [Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "AgriculturalExpert")]
	public class StageController : ControllerBase
	{

		private readonly IServiceManager _serviceManager;

		public StageController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		[HttpPost("AddStage")]
		//[Authorize] // Experts and Farmers can add stages
		public async Task<ActionResult<CropStageDto>> AddStage([FromBody] StageDefinitionDto stageDto)
		{
			if (stageDto == null) return BadRequest("Stage data is required.");
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			try
			{
				var createdStage = await _serviceManager.StageService.AddStageAsync(stageDto, userId);
				// CreatedAtAction uses GetStageById method to return the newly created resource
				return CreatedAtAction(nameof(GetStageById), new { id = createdStage.Id }, createdStage);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while adding the stage: {ex.Message}");
			}
		}

		// Get Stage By Id
		[HttpGet("GetStageById/{id}")]
		public async Task<ActionResult<CropStageDto>> GetStageById(int id)
		{
			try
			{
				var stage = await _serviceManager.StageService.GetStageByIdAsync(id);
				return Ok(stage);
			}
			catch (KeyNotFoundException)
			{
				return NotFound($"Stage with ID {id} not found.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
		// Get All Stages
		[HttpGet("GetStages")]
		public async Task<ActionResult<IReadOnlyList<CropStageDto>>> GetStages()
		{
			var stages = await _serviceManager.StageService.GetAllStagesAsync();
			return Ok(stages);
		}

		//Delete Stage

		[HttpDelete("DeleteStage/{id}")]
		//[Authorize] // Experts and Farmers can delete stages
		public async Task<IActionResult> DeleteStage(int id)
		{
			try
			{
				// Retrieve stage first to ensure existence before passing DTO
				var stage = await _serviceManager.StageService.GetStageByIdAsync(id);
				if (stage == null) return NotFound($"Stage with ID {id} not found.");

				await _serviceManager.StageService.DeleteStage(new CropStageDto { Id = id }); // Pass minimal DTO
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound($"Stage with ID {id} not found for deletion.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting the stage: {ex.Message}");
			}
		}

		//Get Deleted Stages
		[HttpGet("DeletedStages")]
		//[Authorize(Roles = "SystemAdministrator")] // Example: Only System Admins can view deleted items
		public async Task<ActionResult<IReadOnlyList<CropStageDto>>> GetDeletedStages()
		{
			try
			{
				var deletedStages = await _serviceManager.StageService.GetAllDeletedStagesAsync();
				return Ok(deletedStages);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while retrieving deleted stages: {ex.Message}");
			}
		}
	}
}
