using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class LandController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;

		public LandController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		// Get All Lands
		[HttpGet("GetAllLands")]
		public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetLands()
		{
			var lands = await _serviceManager.LandService.GetAllLandsAsync();
			return Ok(lands);
		}

		// Get Land By Id
		[HttpGet("GetLandById/{id}")]
		public async Task<ActionResult<LandDTO>> GetLandById(int id)
		{
			var land = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (land == null)
				return NotFound($"Land with ID {id} not found.");

			return Ok(land);
		}

		// Add Land
		[HttpPost("AddLand")]
		public async Task<ActionResult<LandDTO>> AddLand([FromBody] LandDTO landDto)
		{
			if (landDto is null)
				return BadRequest("Land data is required.");


			var Land = await _serviceManager.LandService.AddAsync(landDto);

			if (Land == null)
				return BadRequest("Failed to create the Land.");

			return CreatedAtAction(nameof(GetLandById), new { id = Land.Id }, Land);
		}



		// Update Land
		[HttpPut("UpdateLandById/{id}")]
		public async Task<IActionResult> UpdateLand(int id, [FromBody] LandDTO landDto)
		{
			if (id != landDto.Id)
				return BadRequest("Land ID mismatch.");

			var existingland = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (existingland == null)
				return NotFound($"Land with ID {id} not found.");

			await _serviceManager.LandService.UpdateLands(landDto);
			return NoContent();
		}



		// Delete Land
		[HttpDelete("DeletLand/{id}")]
		public async Task<IActionResult> DeleteLand(int id)
		{
			var land = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (land == null)
				return NotFound($"Brand with ID {id} not found.");

			await _serviceManager.LandService.DeleteLands(land);
			return NoContent();
		}

	}
}
