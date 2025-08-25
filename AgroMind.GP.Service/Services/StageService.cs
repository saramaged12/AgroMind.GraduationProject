using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
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

		public async Task<CropStageDto> AddStageAsync(StageDefinitionDto stageDto)
		{
			
			var stageEntity = _mapper.Map<CropStage>(stageDto);
		
			stageEntity.ActualCost = 0;
			stageEntity.TotalActualCost = 0; //Initialize actuals for new definition
			if (stageEntity.Steps != null)
			{
				foreach (var step in stageEntity.Steps)
				{
					
					step.ActualCost = null; 
					step.ActualStartDate = null;
				
				}
			}

			
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
			var crop = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification(stageEntity.CropId.Value)); 
			if (crop != null) { stageEntity.Crop = crop; } 
			RecalculateStageCosts(stageEntity);

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			await repo.AddAsync(stageEntity);

			if (stageEntity.CropId.HasValue)
			{
				var CropRepo = _unitOfWork.GetRepositories<Crop, int>();
			
				var cropForRecalc = await cropRepo.GetByIdAWithSpecAsync(new CropSpecification(forUpdate: true, stageEntity.CropId.Value)); 
				if (cropForRecalc != null)
				{ 
				RecalculateCropCosts(cropForRecalc);
				cropRepo.Update(cropForRecalc);
			    }
			}

			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CropStageDto>(stageEntity);
		}

		public async Task<CropStageDto> GetStageByIdAsync(int id)
		{
			var spec = new StageSpecification(id);
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stage = await repo.GetByIdAWithSpecAsync(spec);

			if (stage == null)
				throw new NotFoundException(nameof(CropStage), id);

			return _mapper.Map<CropStage, CropStageDto>(stage);
		}


		public async Task<IReadOnlyList<CropStageDto>> GetAllStagesAsync()
		{
			var Specification = new StageSpecification();
			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var stages = await repo.GetAllWithSpecASync(Specification);
			return _mapper.Map<IReadOnlyList<CropStage>, IReadOnlyList<CropStageDto>>(stages);
		}


		public async Task DeleteStage(int ID)
		{

			var repo = _unitOfWork.GetRepositories<CropStage, int>();
			var existingStage = await repo.GetByIdAsync(ID);
			if (existingStage == null)
				throw new NotFoundException(nameof(Product), ID);
			repo.SoftDelete(existingStage);
			await _unitOfWork.SaveChangesAsync();
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

			// Sum Actual Costs -  for FarmerPlans
			// This requires stage.Crop to be loaded for PlanType check
			if (stage.Crop != null && stage.Crop.PlanType == CropPlanType.FarmerPlan)
			{
				stage.TotalActualCost = stage.ActualCost + (stage.Steps?.Sum(s => s.ActualCost ?? 0) ?? 0);
			}
			else
			{
				// For ExpertTemplates, TotalActualCost should always be 0
				// For other cases ( if PlanType is unknown), default to 0
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