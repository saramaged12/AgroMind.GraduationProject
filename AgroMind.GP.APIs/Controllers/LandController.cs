using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "Farmer")]
	public class LandController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;

		private readonly UserManager<AppUser> _userManager;

		public LandController(IServiceManager serviceManager, UserManager<AppUser> userManager)
		{
			_serviceManager = serviceManager;
			_userManager = userManager;
		}

		// Get All Lands
		[HttpGet("GetAllLands")]
		public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetLands()
		{
			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");


			var lands = await _serviceManager.LandService.GetAllLandsAsync();
			return Ok(lands);
		}

		// Get Land By Id
		[HttpGet("GetLandById/{id}")]
		public async Task<ActionResult<LandDTO>> GetLandById(int id)
		{
			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");


			var land = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (land == null)
				return NotFound($"Land with ID {id} not found.");

			if (land.FarmerId != farmerId)
				return Forbid("You don't have permission to access this land");


			return Ok(land);
		}

		// Add Land
		[HttpPost("AddLand")]
		public async Task<ActionResult<LandDTO>> AddLand([FromBody] LandDTO landDto)
		{
			if (landDto is null)
				return BadRequest("Land data is required.");


			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");

			// Verify farmer exists
			var farmer = await _userManager.FindByIdAsync(farmerId);
			if (farmer == null)
				return NotFound("Farmer not found");

			// Assign land to current farmer
			landDto.FarmerId = farmerId;


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

			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");

			// Verify land exists and belongs to this farmer
			var existingLand = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (existingLand == null)
				return NotFound($"Land with ID {id} not found.");

			if (existingLand.FarmerId != farmerId)
				return Forbid("You don't have permission to update this land");

			// Ensure the farmerId can't be changed
			landDto.FarmerId = farmerId;

			await _serviceManager.LandService.UpdateLands(landDto);
			return NoContent();
		}



		// Delete Land
		[HttpDelete("DeletLand/{id}")]
		public async Task<IActionResult> DeleteLand(int id)
		{
			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");

			var land = await _serviceManager.LandService.GetLandByIdAsync(id);
			if (land == null)
				return NotFound($"Land with ID {id} not found.");

			if (land.FarmerId != farmerId)
				return Forbid("You don't have permission to delete this land");

			await _serviceManager.LandService.DeleteLands(land);
			return NoContent();
		}

		
		[HttpGet("DeletedLands")]
		//[Authorize(Roles = "SystemAdministratot")]
		public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetDeletedLands()
		{
			var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerId))
				return Unauthorized("Invalid user token");

			var deletedLand = await _serviceManager.LandService.GetAllDeletedLandsAsync();
			return Ok(deletedLand);
		}

	}
}
