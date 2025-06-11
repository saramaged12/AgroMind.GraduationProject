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
	public class StepController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public StepController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		//Add Step

		[HttpPost("AddStep")]
		//[Authorize] // Experts and Farmers can add steps
		public async Task<ActionResult<StepDto>> AddStep([FromBody] StepDefinitionDto stepDto)
		{
			if (stepDto == null) return BadRequest("Step data is required.");
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId)) return Unauthorized("User ID not found.");

			try
			{
				var createdStep = await _serviceManager.StepService.AddStepAsync(stepDto, userId);
				// CreatedAtAction uses GetStepById method to return the newly created resource
				return CreatedAtAction(nameof(GetStepById), new { id = createdStep.Id }, createdStep);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while adding the step: {ex.Message}");
			}
		}

		// Get Step By Id
		[HttpGet("GetStepById/{id}")]
		public async Task<ActionResult<StepDto>> GetStepById(int id)
		{
			var step = await _serviceManager.StepService.GetStepByIdAsync(id);
			if (step == null)
				return NotFound($"Step with ID {id} not found.");

			return Ok(step);
		}

		// Get Stage By Id
		[HttpGet("GetStageById/{id}")]
		public async Task<ActionResult<CropStageDto>> GetStageById(int id)
		{
			try
			{
				var step = await _serviceManager.StepService.GetStepByIdAsync(id);
				return Ok(step);
			}
			catch (KeyNotFoundException)
			{
				return NotFound($"Step with ID {id} not found.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
		// Get All Crops
		[HttpGet("GetSteps")]
		public async Task<ActionResult<IReadOnlyList<StepDto>>> GetSteps()
		{
			var steps = await _serviceManager.StepService.GetAllStepsAsync();
			return Ok(steps);
		}

		

		// Delete Step
		[HttpDelete("DeleteStep/{id}")]
		public async Task<IActionResult> DeleteStep(int id)
		{
			try
			{
				// Retrieve step first to ensure existence before passing DTO
				var step = await _serviceManager.StepService.GetStepByIdAsync(id);
				if (step == null) return NotFound($"Step with ID {id} not found.");

				await _serviceManager.StepService.DeleteStep(new StepDto { Id = id }); // Pass minimal DTO
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound($"Step with ID {id} not found for deletion.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting the step: {ex.Message}");
			}
		}

		[HttpGet("GetDeletedSteps")]
		//[Authorize(Roles = "SystemAdministrator")]
		
		public async Task<ActionResult<IReadOnlyList<StepDto>>> GetDeletedSteps()
		{
			try
			{
				var deletedSteps = await _serviceManager.StepService.GetAllDeletedStepsAsync();
				return Ok(deletedSteps);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while retrieving deleted steps: {ex.Message}");
			}
		}
	}
}


