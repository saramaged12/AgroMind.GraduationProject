using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]

	//[Authorize(Roles = "AgriculturalExpert")]
	public class CropController : APIbaseController
	{
		private readonly IGenericRepositories<Crop, int> _croprepo;
		private readonly IMapper _mapper;

		public CropController(IGenericRepositories<Crop, int> croprepo, IMapper mapper)
		{
			_croprepo = croprepo;
			_mapper = mapper;
		}


		//Get All
		[HttpGet("GetAllCrops")]
		public async Task<ActionResult<IEnumerable<Crop>>> GetCrops()
		{
			var Spec = new CropSpecification();
			var crops = await _croprepo.GetAllWithSpecASync(Spec);
			return Ok(crops);

		}

		//Get By Id
		[HttpGet("GetCropById/{id}")]
		public async Task<ActionResult<Crop>> GetCroptById(int id)
		{
			var spec = new CropSpecification(id);
			var crop = await _croprepo.GetByIdAWithSpecAsync(spec);
			return Ok(crop);
		}

		//Add

		[HttpPost("AddCrop")]
		public async Task<ActionResult<CropDto>> AddCrop(CropDto cropDto)
		{
			var crop = _mapper.Map<Crop>(cropDto);
			await _croprepo.AddAsync(crop);

			var resultDto = _mapper.Map<CropDto>(crop);
			return CreatedAtAction(nameof(GetCroptById), new { id = resultDto.Id }, resultDto);
		}


		//Update

		[HttpPut("UpdateCrop/{id}")]
		public async Task<IActionResult> UpdateCrop(int id, CropDto cropDto)
		{
			if (id != cropDto.Id)
			{
				return BadRequest();
			}

			var spec = new CropSpecification(id);
			var existingcrop = await _croprepo.GetByIdAWithSpecAsync(spec);

			if (existingcrop == null)
			{
				return NotFound();
			}

			_mapper.Map(cropDto, existingcrop); // Map DTO to existing entity
			_croprepo.Update(existingcrop);

			return NoContent(); // 204 No Content
		}



		//Delete

		[HttpDelete("DeleteCrop/{id}")]
		public async Task<IActionResult> DeleteCrop(int id)
		{
			var spec = new CropSpecification(id);
			var crop = await _croprepo.GetByIdAWithSpecAsync(spec);

			if (crop == null)
			{
				return NotFound();
			}

			_croprepo.Delete(crop);
			return NoContent(); // 204 No Content
		}

	}
}
