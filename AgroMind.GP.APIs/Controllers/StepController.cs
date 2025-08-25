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
			
			var createdStep = await _serviceManager.StepService.AddStepAsync(stepDto);
				
				return Ok(createdStep);
			
		}

		// Get Step By Id
		[HttpGet("GetStepById/{id}")]
		public async Task<ActionResult<StepDto>> GetStepById(int id)
		{
			var step = await _serviceManager.StepService.GetStepByIdAsync(id);
			
			return Ok(step);
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
				
				await _serviceManager.StepService.DeleteStep(id); // Pass minimal DTO
				return NoContent();
			
			
		}

		[HttpGet("GetDeletedSteps")]
		//[Authorize(Roles = "SystemAdministrator")]
		
		public async Task<ActionResult<IReadOnlyList<StepDto>>> GetDeletedSteps()
		{
			
				var deletedSteps = await _serviceManager.StepService.GetAllDeletedStepsAsync();
				return Ok(deletedSteps);
			
		}
	}
}


