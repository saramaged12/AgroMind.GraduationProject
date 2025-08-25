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
			
			
				var createdStage = await _serviceManager.StageService.AddStageAsync(stageDto);
			
			    return Ok(createdStage);
		}

		// Get Stage By Id
		[HttpGet("GetStageById/{id}")]
		public async Task<ActionResult<CropStageDto>> GetStageById(int id)
		{
			
				var stage = await _serviceManager.StageService.GetStageByIdAsync(id);
				return Ok(stage);
			
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
				
				await _serviceManager.StageService.DeleteStage(id); // Pass minimal DTO
				return NoContent();
			
		}

		//Get Deleted Stages
		[HttpGet("DeletedStages")]
		//[Authorize(Roles = "SystemAdministrator")] // Example: Only System Admins can view deleted items
		public async Task<ActionResult<IReadOnlyList<CropStageDto>>> GetDeletedStages()
		{
				var deletedStages = await _serviceManager.StageService.GetAllDeletedStagesAsync();
				return Ok(deletedStages);
			
		}
	}
}
