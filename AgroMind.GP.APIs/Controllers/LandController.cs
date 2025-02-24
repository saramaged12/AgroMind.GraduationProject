using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LandController : ControllerBase
	{
        private ILandRepository _landRepository;

        public LandController(ILandRepository landRepository)
		{
            _landRepository = landRepository;
		}

		//Get or ReCreate Land
		[HttpGet ("{LandId}")]
		public async Task<ActionResult<Land>> GetLandById(int LandId) //<ActionResult<Land> : will return Land
		{
			var land = await _landRepository.GetLandByIdAsync(LandId); //Law Null don't have cart with this id
			if (land == null) //kant mawogode and it deleted because expire date
			return new Land(LandId); //reCreate > Same Land wit same Id el kant Mawgoda
            return Ok(land);
		}

        //Update or Create New Land
        [HttpPost("CreateLand")]
		public async Task<ActionResult<Land>>UpdateLand(Land land) 
		{
		    var CreatedOrUpdatedLand= await _landRepository.CreateOrUpdateLandtAsync(land);
            //if (CreatedOrUpdatedLand is null) return BadRequest(new BadRequestObjectResult(400)); //Frontend Problem /not create and not update
            //Return BadRequest() for frontend not User 		    
            return Ok(CreatedOrUpdatedLand);
		}

		//Delete Land
		[HttpDelete("{LandId}") ]
		public async Task<ActionResult<bool>> DeleteLand(int LandId) //Deleted or No
		{ 
		   return await _landRepository.DeleteLandByIdAsync(LandId);
		}

	}
}
