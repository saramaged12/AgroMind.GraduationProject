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
	public class StepController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public StepController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		//Add Step

		[HttpPost("AddStep")]
		public async Task<ActionResult<StepDto>> AddStep([FromBody]StepDto stepDto)
		{

			if (stepDto == null)
				return BadRequest(" Step data is required.");

			
			var createdstep = await _serviceManager.StepService.AddStepAsync(stepDto);

			if (createdstep == null)
				return BadRequest("Failed to create the Crop.");

			return CreatedAtAction(nameof(GetStepById), new { id = createdstep.Id }, createdstep);

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

	}
}
