
using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace AgroMind.GP.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandController : APIbaseController
    {
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<AppUser> _userManager; // Inject UserManager to get user ID and check roles

        public LandController(IServiceManager serviceManager, UserManager<AppUser> userManager)
        {
            _serviceManager = serviceManager;
            _userManager = userManager;
        }

        // GET ALL LANDS
        
        [HttpGet("GetAllLands")]
        public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetLands()
        {
            var lands = await _serviceManager.LandService.GetAllLandsAsync();
            return Ok(lands);
        }

        // GET LAND BY ID 
       
        [HttpGet("GetLandById/{id}")]
        public async Task<ActionResult<LandDTO>> GetLandById(int id)
        {
            try
            {
                var land = await _serviceManager.LandService.GetLandByIdAsync(id);
                return Ok(land);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Land with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

		[HttpGet("GetMyLands")]
		[Authorize(Roles = "Farmer")]
		public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetMyLands()
		{
			var farmerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(farmerUserId)) return Unauthorized("Farmer ID not found in token.");

			try
			{
				var myLands = await _serviceManager.LandService.GetMyLandsAsync(farmerUserId);
				return Ok(myLands);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while retrieving your lands: {ex.Message}");
			}
		}

		//  ADD LAND 

		[HttpPost("AddLand")]
        [Authorize(Roles = "Farmer")] 
        public async Task<ActionResult<LandDTO>> AddLand([FromBody] LandDTO landDto)
        {
            if (landDto == null) return BadRequest("Land data is required.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //  Get FarmerId from Authenticated User's Token
            var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(farmerId))
            {
                
                return Unauthorized("Farmer ID not found in token. Please log in as a Farmer.");
            }

            
            var user = await _userManager.FindByIdAsync(farmerId);
            if (user == null) return NotFound("Authenticated user (Farmer) not found in the system."); // Should not happen if token is valid
                                                                                                       // You can add an explicit role check here if needed, but [Authorize(Roles="Farmer")] handles it.
           
            // Assign the FarmerId to the DTO (Override any value sent by client for security)
            landDto.FarmerId = farmerId; //  ASSIGN FROM TOKEN, NOT BODY

        
            try
            {
                var createdLand = await _serviceManager.LandService.AddAsync(landDto);
                return Ok(createdLand);
            }
            catch (Exception ex)
            {
               
                if (ex.InnerException != null) 
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
               
                if (ex is UnauthorizedAccessException) 
                    return Forbid(ex.Message); 
                if (ex is KeyNotFoundException)
                    return NotFound(ex.Message); 
                return StatusCode(500, $"An error occurred while creating the Land: {ex.Message}");
            }
        }

			// UPDATE LAND
			
		[HttpPut("UpdateLandById/{id}")]
        [Authorize(Roles = "Farmer")] // Only the owning farmer can update their land
        public async Task<IActionResult> UpdateLand(int id, [FromBody] LandDTO landDto)
        {
            if (id != landDto.Id) 
                return BadRequest("Land ID mismatch.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get ID of user trying to update
            if (string.IsNullOrEmpty(farmerId))
                return Unauthorized("Farmer ID not found in token.");

            
            
            try
            {
                // Pass farmerId to service for authorization check
                await _serviceManager.LandService.UpdateLands(landDto, farmerId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Land with ID {id} not found.");
            }
            catch (UnauthorizedAccessException ex) // Thrown by service if not authorized
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the Land: {ex.Message}");
            }
        }

        // DELETE LAND 

        [HttpDelete("DeletLand/{id}")]
        [Authorize(Roles = "Farmer")] // Only the owning farmer can delete their land
        public async Task<IActionResult> DeleteLand(int id)
        {
            var farmerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(farmerId))
                return Unauthorized("Farmer ID not found in token.");

            try
            {
                // Pass farmerId to service for authorization check
                await _serviceManager.LandService.DeleteLands(new LandDTO { Id = id }, farmerId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Land with ID {id} not found.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the Land: {ex.Message}");
            }
        }


        // GET DELETED LANDS 
        [HttpGet("DeletedLands")]
        [Authorize(Roles = "SystemAdministrator")] //  only System Admins can see deleted lands
        public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetDeletedLands()
        {
            try
            {
                var deletedLand = await _serviceManager.LandService.GetAllDeletedLandsAsync();
                return Ok(deletedLand);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}