
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
using AgroMind.GP.Core.Exceptions;

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

       
        
        [HttpGet("GetAllLands")]
        public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetLands()
        {
            var lands = await _serviceManager.LandService.GetAllLandsAsync();
            return Ok(lands);
        }

       
       
        [HttpGet("GetLandById/{id}")]
        public async Task<ActionResult<LandDTO>> GetLandById(int id)
        {
                var land = await _serviceManager.LandService.GetLandByIdAsync(id);
                return Ok(land);
          
        }

		[HttpGet("GetMyLands")]
		[Authorize(Roles = "Farmer")]
		public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetMyLands(string FarmerId)
		{
			
				var myLands = await _serviceManager.LandService.GetMyLandsAsync(FarmerId);
				return Ok(myLands);
			
		}

		[HttpPost("AddLand")]
        [Authorize(Roles = "Farmer")] 
        public async Task<ActionResult<LandDTO>> AddLand([FromBody] LandDTO landDto)
        {
         
                var createdLand = await _serviceManager.LandService.AddAsync(landDto);
                return Ok(createdLand);
            
        }

		[HttpPut("UpdateLandById/{id}")]
        [Authorize(Roles = "Farmer")] // Only the owning farmer can update their land
        public async Task<IActionResult> UpdateLand(int id, [FromBody] LandDTO landDto)
        {
            if (id != landDto.Id) 
                throw new BadRequestException("Land ID mismatch.");
           
            var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get ID of user trying to update
           
            await _serviceManager.LandService.UpdateLands(landDto, farmerId);
            return NoContent();
           
        }

       
        [HttpDelete("DeletLand/{id}")]
        [Authorize(Roles = "Farmer")] // Only the owning farmer can delete their land
        public async Task<IActionResult> DeleteLand(int id)
        {
            var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.LandService.DeleteLands(id, farmerId);
            return NoContent();
           
        }


      
        [HttpGet("DeletedLands")]
        [Authorize(Roles = "SystemAdministrator")] //  only System Admins can see deleted lands
        public async Task<ActionResult<IReadOnlyList<LandDTO>>> GetDeletedLands()
        {
           
                var deletedLand = await _serviceManager.LandService.GetAllDeletedLandsAsync();
                return Ok(deletedLand);
          
        }
    }
}