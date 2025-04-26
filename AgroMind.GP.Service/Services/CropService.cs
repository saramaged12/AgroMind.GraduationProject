using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class CropService :ICropService
	{


		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CropService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
	

		public async Task DeleteCrop(CropDto cropDto)
		{
			if (cropDto == null)
				throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var existingCrop = await repo.GetByIdAsync(cropDto.Id);

			if (existingCrop == null)
				throw new KeyNotFoundException($"Crop with ID {cropDto.Id} not found.");

			repo.Delete(existingCrop);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<CropDto>> GetAllCropsAsync()
		{
			var Specification= new CropSpecification();
			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var crops = await repo.GetAllWithSpecASync(Specification);
			return _mapper.Map<IReadOnlyList<Crop>, IReadOnlyList<CropDto>>(crops);
		}

		public async Task<CropDto> GetCropByIdAsync(int id)
		{
			var spec= new CropSpecification(id);
			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var crop = await repo.GetByIdAWithSpecAsync(spec);

			if (crop == null)
				throw new KeyNotFoundException($"Crop with ID {id} not found.");

			return _mapper.Map<Crop, CropDto>(crop);
		}

		public async Task<CropDto> AddCropAsync(CropDto cropDto)
		{
			if (cropDto == null)
				throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

			var cropEntity = _mapper.Map<Crop>(cropDto);

			// No need to explicitly calculate TotalCost; it is a read-only property
			var repo = _unitOfWork.GetRepositories<Crop, int>();
			await repo.AddAsync(cropEntity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CropDto>(cropEntity);
		}

		public async Task UpdateCrops(CropDto cropDto)
		{
			if (cropDto == null)
				throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var existingCrop = await repo.GetByIdAsync(cropDto.Id);

			if (existingCrop == null)
				throw new KeyNotFoundException($"Crop with ID {cropDto.Id} not found.");

			_mapper.Map(cropDto, existingCrop);

			// No need to explicitly calculate TotalCost; it is a read-only property
			repo.Update(existingCrop);
			await _unitOfWork.SaveChangesAsync();
		}

	}
}
