using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


		//AddStage

		[HttpPost("AddStage")]
		public async Task<ActionResult<CropStageDto>> AddStage([FromBody] CropStageDto stageDto)
		{
			if (stageDto == null)
				return BadRequest(" Stage data is required.");

			
			var createdStage = await _serviceManager.StageService.AddStageAsync(stageDto);

			if (createdStage == null)
				return BadRequest("Failed to create the Stage.");

			return CreatedAtAction(nameof(GetStageById), new { id = createdStage.Id }, createdStage);

		}

		// Get Stage By Id
		[HttpGet("GetStageById/{id}")]
		public async Task<ActionResult<CropStageDto>> GetStageById(int id)
		{
			var stage = await _serviceManager.StageService.GetStageByIdAsync(id);
			if (stage == null)
				return NotFound($"Stage with ID {id} not found.");

			return Ok(stage);
		}
		// Get All Stages
		[HttpGet("GetStages")]
		public async Task<ActionResult<IReadOnlyList<CropStageDto>>> GetStages()
		{
			var stages = await _serviceManager.StageService.GetAllStagesAsync();
			return Ok(stages);
		}

		// Update Stage
		[HttpPut("UpdateSatge/{id}")]
		public async Task<IActionResult> UpdateStage(int id, [FromBody] CropStageDto stageDto)
		{
			if (id != stageDto.Id)
				return BadRequest("Stage ID mismatch.");

			var existingStage = await _serviceManager.StageService.GetStageByIdAsync(id);
			if (existingStage == null)
				return NotFound($"Stage with ID {id} not found.");

			await _serviceManager.StageService.UpdateStage(stageDto);
			return NoContent();
		}

		// Delete Stage
		[HttpDelete("DeleteStage/{id}")]
		public async Task<IActionResult> DeleteStage(int id)
		{
			var stage = await _serviceManager.StageService.GetStageByIdAsync(id);
			if (stage == null)
				return NotFound($"Stage with ID {id} not found.");

			await _serviceManager.StageService.DeleteStage(new CropStageDto { Id = id });
			return NoContent();
		}
	}
}
