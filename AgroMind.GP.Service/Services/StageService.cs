using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class StageService : IStageService
	{
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;

		public StageService(IUnitOfWork unitOfWork, IMapper mapper,UserManager<AppUser> userManager)
		{
			_mapper = mapper;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
		}

		public async Task DeleteStage(CropStageDto stageDto)
		{
			if (stageDto == null)
				throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var existingStage = await repo.GetByIdAsync(stageDto.Id);

			if (existingStage == null)
				throw new KeyNotFoundException($"Stage with ID {stageDto.Id} not found.");

			repo.SoftDelete(existingStage);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<CropStageDto>> GetAllStagesAsync()
		{
			var Specification = new StageSpecification();
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stages = await repo.GetAllWithSpecASync(Specification);
			return _mapper.Map<IReadOnlyList<CropStage>, IReadOnlyList<CropStageDto>>(stages);
		}

		public async Task<CropStageDto> GetStageByIdAsync(int id)
		{
			var spec = new StageSpecification(id);
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stage = await repo.GetByIdAWithSpecAsync(spec);

			if (stage == null)
				throw new KeyNotFoundException($"Stage with ID {id} not found.");

			return _mapper.Map<CropStage, CropStageDto>(stage);
		}




		public async Task<CropStageDto> AddStageAsync(StageDefinitionDto stageDto, string CreatorUserId)
		{
			if (stageDto == null)
				throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

			var stageEntity = _mapper.Map<CropStage>(stageDto);
			stageEntity.CreatorId = CreatorUserId;

			stageEntity.ActualCost = 0;
			stageEntity.TotalActualCost = 0; // Initialize actuals for new definition
			if (stageEntity.Steps != null)
			{
				foreach (var step in stageEntity.Steps)
				{
					step.CreatorId = CreatorUserId;
					step.ActualCost = null; 
					step.ActualStartDate = null;
					//step.PlannedStartDate = null;
				}
			}

			// Calculate TotalCost for the stage
			//stageEntity.TotalEstimatedCost = stageEntity.EstimatedCost + (stageEntity.Steps?.Sum(step => step.EstimatedCost) ?? 0);

			//var repo = _unitOfWork.GetRepositories<CropStage, int>();
			//await repo.AddAsync(stageEntity);

			//// Update  crop TotalCost 
			//if (stageEntity.CropId.HasValue)
			//{
			//	var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
			//	var crop = await cropRepo.GetByIdAsync(stageEntity.CropId.Value);
			//	if (crop != null)
			//	{
			//		//crop.TotalCost = crop.Stages?.Sum(stage => stage.TotalCost) ?? 0;
			//		crop.TotalEstimatedCost = crop.Stages?.Sum(s =>
			//		   s.EstimatedCost + (s.Steps?.Sum(step => step.EstimatedCost) ?? 0)) ?? 0;

			//		cropRepo.Update(crop); // Save changes to the crop
			//	}
			//}

			// Load parent Crop for accurate recalculation based on PlanType.
			// Use specification for loading stage with its crop
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
			var crop = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification(stageEntity.CropId.Value)); // Load crop (default spec includes stages/steps)
			if (crop != null) { stageEntity.Crop = crop; } // Temporarily assign for recalculation helper
			RecalculateStageCosts(stageEntity);

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			await repo.AddAsync(stageEntity);

			if (stageEntity.CropId.HasValue)
			{
				var CropRepo = _unitOfWork.GetRepositories<Crop, int>();
				// Load crop with its stages/steps using a spec for recalculation
				var cropForRecalc = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification(forUpdate: true, stageEntity.CropId.Value)); // Use forUpdate spec
				if (cropForRecalc != null)
				{ 
				RecalculateCropCosts(cropForRecalc);
				cropRepo.Update(cropForRecalc);
			    }
			}

			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CropStageDto>(stageEntity);
		}

		



		public async Task<IReadOnlyList<CropStageDto>> GetAllDeletedStagesAsync()
		{
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var DeletedStages = await repo.GetAllDeletedAsync();
			return _mapper.Map<IReadOnlyList<CropStageDto>>(DeletedStages);
		}

		private void RecalculateStageCosts(CropStage stage)
		{
			// Sum Estimated Costs - always calculated
			stage.TotalEstimatedCost = stage.EstimatedCost + (stage.Steps?.Sum(s => s.EstimatedCost ?? 0) ?? 0);

			// Sum Actual Costs - ONLY for FarmerPlans
			// This requires stage.Crop to be loaded for PlanType check.
			if (stage.Crop != null && stage.Crop.PlanType == CropPlanType.FarmerPlan)
			{
				stage.TotalActualCost = stage.ActualCost + (stage.Steps?.Sum(s => s.ActualCost ?? 0) ?? 0);
			}
			else
			{
				// For ExpertTemplates, TotalActualCost should always be 0
				// For other cases (e.g., if PlanType is unknown), default to 0
				stage.TotalActualCost = 0;
			}
		}

		private void RecalculateCropCosts(Crop crop)
		{
			if (crop.Stages != null)
			{
				foreach (var stage in crop.Stages)
				{
					// Temporarily set the parent Crop on the stage for helper function to check PlanType
					stage.Crop = crop; // This avoids extra DB calls if crop is already loaded
					RecalculateStageCosts(stage);
				}
			}
			// Sum Estimated Costs - always calculated
			crop.TotalEstimatedCost = crop.Stages?.Sum(s => s.TotalEstimatedCost ?? 0) ?? 0;

			// Sum Actual Costs - ONLY for FarmerPlans
			if (crop.PlanType == CropPlanType.FarmerPlan)
			{
				crop.TotalActualCost = crop.Stages?.Sum(s => s.TotalActualCost ?? 0) ?? 0;
			}
			else
			{
				crop.TotalActualCost = 0; // ExpertTemplates, TotalActualCost is 0
			}
		}

		//public async Task UpdateStage(CropStageDto stageDto)
		//{
		//	if (stageDto == null) throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

		//	var repo = _unitOfWork.GetRepositories<CropStage, int>();
		//	// Load existing stage with steps and parent crop for correct recalculation
		//	var spec = new StageSpecification(stageDto.Id, forUpdate: true); // Use forUpdate spec
		//	var existingStage = await repo.GetByIdAWithSpecAsync(spec);

		//	if (existingStage == null) throw new KeyNotFoundException($"Stage with ID {stageDto.Id} not found.");

		//	_mapper.Map(stageDto, existingStage); // Update scalar properties

		//	//var currentUserId = _userManager.GetUserId(); // Get current user ID for creator fields

		//	// Manage steps (add/update/delete) - simpler approach
		//	existingStage.Steps.RemoveAll(st => !stageDto.Steps.Any(dtoSt => dtoSt.Id == st.Id));
		//	foreach (var dtoStep in stageDto.Steps)
		//	{
		//		var existingStep = existingStage.Steps.FirstOrDefault(st => st.Id == dtoStep.Id);
		//		if (existingStep == null) // New step
		//		{
		//			existingStep = _mapper.Map<Step>(dtoStep);
		//			//existingStep.CreatorId = currentUserId;
		//			existingStep.ActualCost = null; existingStep.ActualStartDate = null; 
		//			existingStep.PlannedStartDate = null;
		//			existingStage.Steps.Add(existingStep);
		//		}
		//		_mapper.Map(dtoStep, existingStep); // Update scalar properties
		//	}

		//	RecalculateStageCosts(existingStage); // Recalculate stage totals

		//	if (existingStage.CropId.HasValue)
		//	{
		//		var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
		//		var crop = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification( forUpdate: true, existingStage.CropId.Value)); // Use forUpdate spec
		//		if (crop != null) 
		//		 RecalculateCropCosts(crop); cropRepo.Update(crop); 
		//	}

		//	repo.Update(existingStage);
		//	await _unitOfWork.SaveChangesAsync();
		//}


		//public async Task UpdateStage(CropStageDto stageDto)
		//{
		//	if (stageDto == null)
		//		throw new ArgumentNullException(nameof(stageDto), "Stage data cannot be null.");

		//	var repo = _unitOfWork.GetRepositories<CropStage, int>();
		//	var existingStage = await repo.GetByIdAsync(stageDto.Id);

		//	if (existingStage == null)
		//		throw new KeyNotFoundException($"Stage with ID {stageDto.Id} not found.");

		//	_mapper.Map(stageDto, existingStage);

		//	// Recalculate TotalCost for the stage
		//	existingStage.TotalEstimatedCost = existingStage.EstimatedCost + (existingStage.Steps?.Sum(step => step.EstimatedCost) ?? 0);
		//	// Update the crop TotalCost 
		//	if (existingStage.CropId.HasValue)
		//	{
		//		var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
		//		var crop = await cropRepo.GetByIdAsync(existingStage.CropId.Value);
		//		if (crop != null)
		//		{
		//			//crop.TotalCost = crop.Stages?.Sum(stage => stage.TotalCost) ?? 0;

		//			crop.TotalEstimatedCost = crop.Stages?.Sum(s =>
		//			   s.EstimatedCost + (s.Steps?.Sum(step => step.EstimatedCost) ?? 0)) ?? 0;

		//			cropRepo.Update(crop); // Save changes to the crop
		//		}
		//	}

		//	repo.Update(existingStage);

		//	await _unitOfWork.SaveChangesAsync();
		//}

	}
}