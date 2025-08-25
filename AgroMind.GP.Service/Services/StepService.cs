using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Exceptions;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class StepService : IStepService
	{
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;

		public StepService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
		{
			_mapper = mapper;
			_userManager = userManager;
			_unitOfWork = unitOfWork;
		}


		
		public async Task<IReadOnlyList<StepDto>> GetAllStepsAsync()
		{

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var steps = await repo.GetAllAsync();
			return _mapper.Map<IReadOnlyList<Step>, IReadOnlyList<StepDto>>(steps);
		}

		public async Task<StepDto> GetStepByIdAsync(int id)
		{

			var repo = _unitOfWork.GetRepositories<Step, int>();
			var step = await repo.GetByIdAsync(id);

			if (step == null)
				throw new NotFoundException(nameof(Step), id);

			return _mapper.Map<Step, StepDto>(step);
		}




		public async Task<StepDto> AddStepAsync(StepDefinitionDto stepDto)
		{
			
			var stepEntity = _mapper.Map<Step>(stepDto);
			

			stepEntity.ActualCost = null;
			stepEntity.ActualStartDate = null;
			

			var repo = _unitOfWork.GetRepositories<Step, int>();
			await repo.AddAsync(stepEntity);

			// Update TotalCost of the parent stage and Crop
			if (stepEntity.StageId.HasValue)
			{
				var stageRepo = _unitOfWork.GetRepositories<CropStage, int>();
				// Use specification to load stage with its steps AND its parent Crop for correct recalculation
				var stage = await stageRepo.GetByIdAWithSpecAsync(new StageSpecification(stepEntity.StageId.Value, forUpdate: true)); // Use forUpdate spec
				if (stage != null)
				{

					RecalculateStageCosts(stage);
					stageRepo.Update(stage);
					if (stage.CropId.HasValue)
					{
						var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
						// Use specification to load crop with its stages/steps for recalculation
						var crop = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification( forUpdate: true, stage.CropId.Value)); // Use forUpdate spec
						if (crop != null)
							RecalculateCropCosts(crop); cropRepo.Update(crop);
					}
				}
			}

			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<StepDto>(stepEntity);
		}


		public async Task DeleteStep(int id)
		{
			
			var repo = _unitOfWork.GetRepositories<Step, int>();
			var existingStep = await repo.GetByIdAsync(id);

			if (existingStep == null)
				throw new NotFoundException(nameof(Step),id);

			repo.SoftDelete(existingStep);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<StepDto>> GetAllDeletedStepsAsync()
		{
			var repo = _unitOfWork.GetRepositories<Step, int>();
			var deletedSteps = await repo.GetAllDeletedAsync();
			return _mapper.Map<IReadOnlyList<StepDto>>(deletedSteps);
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
	}
}