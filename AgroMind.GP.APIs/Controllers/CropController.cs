using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	
	public class CropController : APIbaseController
	{
		private readonly IGenericRepositories<Crop, int> _croprepo;
		
		public CropController(IGenericRepositories<Crop, int> croprepo)
		{
			_croprepo = croprepo;
		
		}


		//Get All
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Crop>>> GetCrops()
		{
			var Spec = new CropSpecification();
			var crops = await _croprepo.GetAllWithSpecASync(Spec);
			return Ok(crops);

		}

		//Get By Id
		[HttpGet("{id}")]
		public async Task<ActionResult<Crop>> GetCroptById(int id)
		{
			var spec = new CropSpecification(id);
			var crop = await _croprepo.GetByIdAWithSpecAsync(spec);
			return Ok(crop);
		}

		//Add


		//Update


		//Delet

	}
}
