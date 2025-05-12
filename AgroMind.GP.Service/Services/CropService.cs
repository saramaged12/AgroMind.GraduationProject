using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Shared.DTOs;
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

			repo.SoftDelete(existingCrop);
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


			// Set TotalCost for each stage:
			if (cropEntity.Stages != null)
			{
				foreach (var stage in cropEntity.Stages)
				{
					stage.TotalCost = stage.Cost + (stage.Steps?.Sum(s => s.Cost) ?? 0);
				}
			}
			//cropEntity.TotalCost=cropEntity.Stages?.Sum(s=>s.TotalCost) ??0; 
			// Calculate TotalCost for the crop based on its stages
			//This not Store Valu in DB (NOT Valid)

			// Calculate TotalCost for the crop's stages and steps
			cropEntity.TotalCost = cropEntity.Stages?.Sum(stage =>
				stage.Cost + (stage.Steps?.Sum(step => step.Cost) ?? 0)) ?? 0;


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

			// Set TotalCost for each stage:
			if (existingCrop.Stages != null)
			{
				foreach (var stage in existingCrop.Stages)
				{
					stage.TotalCost = stage.Cost + (stage.Steps?.Sum(s => s.Cost) ?? 0);
				}
			}

			
			//existingCrop.TotalCost = existingCrop.Stages?.Sum(stage => stage.TotalCost) ?? 0;


			// Recalculate TotalCost for the crop
			existingCrop.TotalCost = existingCrop.Stages?.Sum(stage =>
				stage.Cost + (stage.Steps?.Sum(step => step.Cost) ?? 0)) ?? 0;

			repo.Update(existingCrop);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<CropDto>> GetAllDeletedCropsAsync()
		{
			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var deletedcrops = await repo.GetAllDeletedAsync();
			return _mapper.Map<IReadOnlyList<CropDto>>(deletedcrops);
		}

		public async Task<IReadOnlyList<CropDto>> GetRecommendedCropsAsync(RecommendRequestDTO recommendDto)
		{
			if (recommendDto.FromDate >= recommendDto.ToDate)
				throw new ArgumentException("The ToDate must be after the FromDate.");

			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var allCrops = await repo.GetAllWithSpecASync(new CropSpecification()); 

			var matchingCrops = new List<Crop>();
			var failedReasons = new List<string>();

			foreach (var crop in allCrops)
			{
				//  earliest and latest possible planting dates 
				var earliestStart = crop.StartDate > recommendDto.FromDate ? crop.StartDate : recommendDto.FromDate;
				// LastStart is the minimum of crop.LastStartDate and Land ToDate - duration
				var landLastValidStart = recommendDto.ToDate.AddDays(-crop.Duration);
				var latestStart = crop.LastStartDate < landLastValidStart ? crop.LastStartDate : landLastValidStart;

				// if there is at least one valid planting day
				if (earliestStart > latestStart)
				{
					failedReasons.Add($"{crop.CropName}: No valid planting window for this crop in the given date range. (Earliest: {earliestStart:yyyy-MM-dd}, Latest: {latestStart:yyyy-MM-dd})");
					continue;
				}
				// check for Budget
				if (recommendDto.Budget < crop.TotalCost)
				{
					failedReasons.Add($"{crop.CropName}: Budget ({recommendDto.Budget}) is less than the total cost of the crop ({crop.TotalCost}).");
					continue;
				}
				matchingCrops.Add(crop);
			}

			if (!matchingCrops.Any())
				throw new RecommendationException("No suitable crops found.", failedReasons);

			
			var sorted = matchingCrops
	            .OrderBy(c => c.TotalCost) // Cheapest first
	            .ToList();

			return _mapper.Map<IReadOnlyList<CropDto>>(sorted);

		}



	}
}
