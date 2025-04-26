using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgroMind.GP.APIs.Controllers
{

    [Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "AgriculturalExpert")]
	public class CropController : APIbaseController
	{
		private readonly IServiceManager _serviceManager;

		public CropController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		// Get All Crops
		[HttpGet("GetCrops")]
		public async Task<ActionResult<IReadOnlyList<CropDto>>> GetCropss()
		{
			var crops = await _serviceManager.CropService.GetAllCropsAsync();
			return Ok(crops);
		}

		// Get Crop By Id
		[HttpGet("GetCropById/{id}")]
		public async Task<ActionResult<CropDto>> GetCropById(int id)
		{
			var crop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (crop == null)
				return NotFound($"Crop with ID {id} not found.");

			return Ok(crop);
		}

		// Add Crop
		[HttpPost("AddCrop")]
		public async Task<ActionResult<CropDto>> AddCrop([FromBody] CropDto cropDto)
		{
			if (cropDto == null)
				return BadRequest(" Crop data is required.");

			// Call the service to add the Crop and return the created Crop
			var createdcrop = await _serviceManager.CropService.AddCropAsync(cropDto);

			if (createdcrop == null)
				return BadRequest("Failed to create the Crop.");

			return CreatedAtAction(nameof(GetCropById), new { id = createdcrop.Id }, createdcrop);

		}

		// Update Crop
		[HttpPut("UpdateCrop/{id}")]
		public async Task<IActionResult> UpdateCrop(int id, [FromBody] CropDto cropDto)
		{
			if (id != cropDto.Id)
				return BadRequest("Crop ID mismatch.");

			var existingCrop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (existingCrop == null)
				return NotFound($"Crop with ID {id} not found.");

			await _serviceManager.CropService.UpdateCrops(cropDto);
			return NoContent();
		}

		// Delete Crop
		[HttpDelete("DeleteCrop/{id}")]
		public async Task<IActionResult> DeleteCrop(int id)
		{
			var crop = await _serviceManager.CropService.GetCropByIdAsync(id);
			if (crop == null)
				return NotFound($"Crop with ID {id} not found.");

			await _serviceManager.CropService.DeleteCrop(new CropDto { Id = id });
			return NoContent();
		}
		//private readonly IGenericRepositories<Crop, int> _croprepo;
		//private readonly IMapper _mapper;

		//public CropController(IGenericRepositories<Crop, int> croprepo, IMapper mapper)
		//{
		//	_croprepo = croprepo;
		//	_mapper = mapper;
		//}


		////Get All
		//[HttpGet("GetAllCrops")]
		//public async Task<ActionResult<IReadOnlyList<Crop>>> GetCrops()
		//{
		//	var Spec = new CropSpecification();
		//	var crops = await _croprepo.GetAllWithSpecASync(Spec);
		//	var cropDtos = _mapper.Map<List<CropDto>>(crops);
		//	return Ok(cropDtos);

		//}

		////Get By Id
		//[HttpGet("GetCropById/{id}")]
		//public async Task<ActionResult<Crop>> GetCroptById([FromRoute] int id)
		//{
		//	var spec = new CropSpecification(id);
		//	var crop = await _croprepo.GetByIdAWithSpecAsync(spec);
		//	var cropDtos = _mapper.Map<CropDto>(crop);
		//	return Ok(cropDtos);
		//}

		////Add

		//[HttpPost("AddCrop")]
		//public async Task<ActionResult<CropDto>> AddCrop([FromBody] CropDto cropDto)
		//{
		//	//// Deserialize the stages from JSON
		//	//if (!string.IsNullOrEmpty(cropDto.StagesJson)) // Assuming you'll add a StagesJson property to CropDto
		//	//{
		//	//	cropDto.Stages = JsonConvert.DeserializeObject<List<CropStageDto>>(cropDto.StagesJson);
		//	//}

		//	var crop = _mapper.Map<Crop>(cropDto);
		//	await _croprepo.AddAsync(crop);

		//	var resultDto = _mapper.Map<CropDto>(crop);
		//	return Ok(resultDto);
		//}


		////Update

		//[HttpPut("UpdateCrop/{id}")]
		//public async Task<IActionResult> UpdateCrop([FromRoute] int id, [FromBody] CropDto cropDto)
		//{
		//	//if (id != cropDto.Id)
		//	//{
		//	//	return BadRequest();
		//	//}

		//	var spec = new CropSpecification(id);
		//	var existingcrop = await _croprepo.GetByIdAWithSpecAsync(spec);

		//	if (existingcrop == null)
		//	{
		//		return NotFound();
		//	}

		//	_mapper.Map(cropDto, existingcrop); // Map DTO to existing entity
		//	 _croprepo.Update(existingcrop);

		//	return Ok(); // 204 No Content
		//}



		////Delete

		//[HttpDelete("DeleteCrop/{id}")]
		//public async Task<IActionResult> DeleteCrop([FromRoute] int id)
		//{
		//	var spec = new CropSpecification(id);
		//	var crop = await _croprepo.GetByIdAWithSpecAsync(spec);

		//	if (crop == null)
		//	{
		//		return NotFound();
		//	}

		//	 _croprepo.Delete(crop);
		//	return NoContent(); // 204 No Content
		//}

	}

	//POST	Create	Body (JSON)	[FromBody]

	//PUT	Update	Route (id) + Body	[FromRoute] + [FromBody]

	//DELETE	Delete	Route (id)	[FromRoute]

	//GET	Read / Fetch	Route or Query String	[FromRoute] or [FromQuery]

}
