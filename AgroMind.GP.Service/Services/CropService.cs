using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.APIs.Helpers;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.Identity;
using AgroMind.GP.Core.Entities.ProductModule;
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
	public class CropService : ICropService
	{


		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		public CropService(IUnitOfWork unitOfWork, IMapper mapper , UserManager<AppUser> userManager)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userManager = userManager;
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
				if (recommendDto.Budget < crop.TotalEstimatedCost)
				{
					failedReasons.Add($"{crop.CropName}: Budget ({recommendDto.Budget}) is less than the total cost of the crop ({crop.TotalEstimatedCost}).");
					continue;
				}
				matchingCrops.Add(crop);
			}

			if (!matchingCrops.Any())
				throw new RecommendationException("No suitable crops found.", failedReasons);


			var sorted = matchingCrops
				.OrderBy(c => c.TotalEstimatedCost) // Cheapest first
				.ToList();

			return _mapper.Map<IReadOnlyList<CropDto>>(sorted);

		}

		//Get SpecicifPlan And Creator Info 

		public async Task<PlanInfoDto> GetPlanAndCreatorInfoAsync(int cropId)
		{
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
			//  specification constructor to include Creator and Land
			var spec = new CropSpecification(cropId, includeCreatorInfo: true);

			var crop = await cropRepo.GetByIdAWithSpecAsync(spec); 

			if (crop == null)

				throw new KeyNotFoundException($"Plan (Crop) with ID {cropId} not found.");

			string creatorRole = "Unknown";
			if (crop.Creator != null)
			{
				var roles = await _userManager.GetRolesAsync(crop.Creator); //GetRole(User)
				creatorRole = roles.FirstOrDefault() ?? "No Role";
			}

			return new PlanInfoDto
			{
				Crop = _mapper.Map<CropDto>(crop), // Map the full Crop entity to its DTO
				CreatorEmail = crop.Creator?.Email,
				CreatorRole = creatorRole,
				PlanType = crop.PlanType?.ToString() ?? "Unknown"
			};
		}


		public async Task<IReadOnlyList<PlanInfoDto>> GetAllPlansWithCreatorInfoAsync()
		{
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();

			// Use the new specification constructor designed for this purpose
			var spec = new CropSpecification(includeCreatorAndLandForAll: true);

			var allCrops = await cropRepo.GetAllWithSpecASync(spec);

			var planInfoList = new List<PlanInfoDto>();

			foreach (var crop in allCrops)
			{
				string creatorRole = "Unknown";
				if (crop.Creator != null)
				{
					var roles = await _userManager.GetRolesAsync(crop.Creator);
					creatorRole = roles.FirstOrDefault() ?? "No Role";
				}

				planInfoList.Add(new PlanInfoDto
				{
					Crop = _mapper.Map<CropDto>(crop),
					CreatorEmail = crop.Creator?.Email,
					CreatorRole = creatorRole,
					PlanType = crop.PlanType?.ToString() ?? "Unknown"
				});
			}
			return planInfoList;
		}
		// --- NEW: Get My Plans (for farmer's profile "View My Plans") (Now uses Specification) ---
		public async Task<IReadOnlyList<CropDto>> GetMyPlansAsync(string farmerUserId)
		{
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();

			var spec = new CropSpecification(farmerUserId, forMyPlans: true);

			var myPlans = await cropRepo.GetAllWithSpecASync(spec);

			return _mapper.Map<IReadOnlyList<Crop>, IReadOnlyList<CropDto>>(myPlans);
		}



		public async Task<CropDto> AddCropAsync(CropDefinitionDto cropDto, string creatorUserId, string creatorRole)
		{
			if (cropDto == null)
				throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

			var cropEntity = _mapper.Map<Crop>(cropDto);

			cropEntity.CreatorId = creatorUserId;

			// Determine PlanType
			if (cropDto.LandId.HasValue)
			
				cropEntity.PlanType = CropPlanType.FarmerPlan; 
			else
			{
				if (creatorRole == "AgriculturalExpert") 
				 cropEntity.PlanType = CropPlanType.ExpertTemplate; 
				else
					throw new UnauthorizedAccessException("Only Agricultural Experts can create general crop templates without a specific land."); 
			}

			// Initialize actual cost fields for new definitions to 0 or null
			cropEntity.TotalActualCost = 0;

			//// Set TotalCost for each stage:
			//if (cropEntity.Stages != null)
			//{
			//	foreach (var stage in cropEntity.Stages)
			//	{
			//		stage.TotalEstimatedCost = stage.EstimatedCost + (stage.Steps?.Sum(s => s.EstimatedCost) ?? 0);
			//	}
			//}
			////cropEntity.TotalCost=cropEntity.Stages?.Sum(s=>s.TotalCost) ??0; 
			//// Calculate TotalCost for the crop based on its stages
			////This not Store Valu in DB (NOT Valid)

			//// Calculate TotalCost for the crop's stages and steps
			//cropEntity.TotalEstimatedCost = cropEntity.Stages?.Sum(stage =>
			//	stage.EstimatedCost + (stage.Steps?.Sum(step => step.EstimatedCost) ?? 0)) ?? 0;


			if (cropEntity.Stages != null)
			{
				foreach (var stage in cropEntity.Stages)
				{
					stage.CreatorId = creatorUserId;
					stage.ActualCost = 0;
					stage.TotalActualCost = 0;
					if (stage.Steps != null)
					{
						foreach (var step in stage.Steps)
						{
							step.CreatorId = creatorUserId;
							step.ActualCost = null; 
							step.ActualStartDate = null;
							//step.PlannedStartDate = null;
						}
					}
				}
			}
			RecalculateCropCosts(cropEntity); // Calculates all TotalEstimatedCost
											  // and ensures TotalActualCost is 0
											  

			var repo = _unitOfWork.GetRepositories<Crop, int>();
			await repo.AddAsync(cropEntity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CropDto>(cropEntity);
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
		public async Task<IReadOnlyList<CropDto>> GetAllDeletedCropsAsync()
		{
			var repo = _unitOfWork.GetRepositories<Crop, int>();
			var deletedcrops = await repo.GetAllDeletedAsync();
			return _mapper.Map<IReadOnlyList<CropDto>>(deletedcrops);
		}



		//ADOPT RECOMMENDED CROP (Farmer clicks "Apply Plan" on a recommended plan)
		
		public async Task<CropDto> AdoptRecommendedCropAsync(int recommendedCropId, string farmerUserId)
		{
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();
			// Use specification to load recommended crop with full graph
			var spec = new CropSpecification(recommendedCropId); // Default spec includes stages/steps
			var recommendedCrop = await cropRepo.GetByIdAWithSpecAsync(spec);

			if (recommendedCrop == null || recommendedCrop.IsDeleted)
				throw new KeyNotFoundException($"Recommended Crop with ID {recommendedCropId} not found or is deleted.");


			//if (recommendedCrop.PlanType != CropPlanType.ExpertTemplate)
			//	throw new InvalidOperationException($"Only Expert Templates can be adopted. This crop is of type: {recommendedCrop.PlanType}");

			var farmer = await _userManager.FindByIdAsync(farmerUserId) as Farmer;
			if (farmer == null || !farmer.Lands.Any())

				throw new InvalidOperationException("Farmer has no associated land to adopt a plan.");

			int targetLandId = farmer.Lands.First().Id;


			// DEEP COPY: Create a NEW Crop entity for the farmer
			var farmerCrop = _mapper.Map<Crop>(recommendedCrop); // Map core properties
			farmerCrop.Id = 0; // Ensure it's a new entity
			farmerCrop.LandId = targetLandId;
			farmerCrop.PlanType = CropPlanType.FarmerPlan;
			farmerCrop.CreatorId = farmerUserId;
			farmerCrop.TotalActualCost = 0; // NEW adopted plan starts with 0 actual cost

			if (farmerCrop.Stages != null)
			{
				foreach (var stage in farmerCrop.Stages)
				{
					stage.Id = 0;
					stage.CreatorId = farmerUserId;
					stage.ActualCost = 0;
					stage.TotalActualCost = 0;
					if (stage.Steps != null)
					{
						foreach (var step in stage.Steps)
						{
							step.Id = 0; 
							step.CreatorId = farmerUserId; 
							step.ActualCost = null;
							step.ActualStartDate = null;
						}
					}
				}
			}
			RecalculateCropCosts(farmerCrop); 

			await cropRepo.AddAsync(farmerCrop);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CropDto>(farmerCrop);

			
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
				// For ExpertTemplates TotalActualCost should always be 0
				// For other cases if PlanType is unknown default to 0
				stage.TotalActualCost = 0;
			}
		}

		private void RecalculateCropCosts(Crop crop)
		{
			if (crop.Stages != null)
			{
				foreach (var stage in crop.Stages)
				{
					
					stage.Crop = crop; // avoids extra DB calls if crop is already loaded
					RecalculateStageCosts(stage);
				}
			}
			// Sum Estimated Costs - always calculated
			crop.TotalEstimatedCost = crop.Stages?.Sum(s => s.TotalEstimatedCost ?? 0) ?? 0;

			// Sum Actual Costs - ONLY for FarmerPlans
			if (crop.PlanType == CropPlanType.FarmerPlan)
			
				crop.TotalActualCost = crop.Stages?.Sum(s => s.TotalActualCost ?? 0) ?? 0;
			
			else
			
				crop.TotalActualCost = 0; // ExpertTemplates, TotalActualCost is 0
			
		}
		


		public async Task UpdateActualsForCropAsync(int cropId, CropDto cropDtoWithActuals, string modifierUserId)
		{
			
			var cropRepo = _unitOfWork.GetRepositories<Crop, int>();

			// Load the Existing Crop Plan 
			// The specification loads the Crop, its Land (for authorization), and ALL nested Stages/Steps.
			var spec = new CropSpecification(forUpdate: true, cropId);
			var existingCrop = await cropRepo.GetByIdAWithSpecAsync(spec);

			
			if (existingCrop == null)
				throw new KeyNotFoundException($"Crop plan with ID {cropId} not found.");

			// Authorization and Plan Type Check
			
			//if (existingCrop.PlanType != CropPlanType.FarmerPlan)
			//throw new InvalidOperationException($"Actual costs can only be recorded for Farmer Plans. This plan is of type: {existingCrop.PlanType}.");

			// Authenticate and authorize the user trying to modify
			var user = await _userManager.FindByIdAsync(modifierUserId);
			if (user == null)
				throw new UnauthorizedAccessException("Modifier user not found.");
			var userRoles = await _userManager.GetRolesAsync(user);

			// Check if the user is the owning farmer OR an Agricultural Expert.
			bool isOwningFarmer = (existingCrop.Land != null && existingCrop.Land.FarmerId == modifierUserId);
			bool isExpert = userRoles.Contains("AgriculturalExpert");

			if (!isOwningFarmer && !isExpert)
				throw new UnauthorizedAccessException($"User {user.Email} is not authorized to update actuals.");


			// Update (Estimated & Actual)
			_mapper.Map(cropDtoWithActuals, existingCrop);

			// Iterate and Update Nested Stages & Steps
			foreach (var dtoStage in cropDtoWithActuals.Stages)
			{
				
				var existingStage = existingCrop.Stages.FirstOrDefault(s => s.Id == dtoStage.Id);
			
				if (existingStage == null)
					continue;

				// Update Stage's properties (EstimatedCost, ActualCost)
				// The CropStageDto -> CropStage mapping is configured to map these.
				_mapper.Map(dtoStage, existingStage);

				foreach (var dtoStep in dtoStage.Steps)
				{
					// Find the corresponding step in the loaded existingStage
					var existingStep = existingStage.Steps.FirstOrDefault(st => st.Id == dtoStep.Id);
					if (existingStep == null) 
						continue;

					// Update Step's properties 
					// The StepDto -> Step mapping is configured for this.
					_mapper.Map(dtoStep, existingStep);
				}
			}

			// Recalculate Aggregated Costs (Estimated & Actual)
			
			RecalculateCropCosts(existingCrop);

			
			cropRepo.Update(existingCrop);
			await _unitOfWork.SaveChangesAsync(); // All changes are saved in one transaction.
		}


		//public async Task UpdateCrops(CropDefinitionDto cropDto, string modifierUserId) 
		//{
		//	if (cropDto == null) 
		//		throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

		//	var repo = _unitOfWork.GetRepositories<Crop, int>();
		//	var spec = new CropSpecification(forUpdate: true ,cropDto.Id);
		//	var existingCrop = await repo.GetByIdAWithSpecAsync(spec);

		//	if (existingCrop == null)
		//		throw new KeyNotFoundException($"Crop with ID {cropDto.Id} not found.");

		//	_mapper.Map(cropDto, existingCrop);

		//	var currentUserId = modifierUserId; 
		//	if (string.IsNullOrEmpty(currentUserId)) 
		//		throw new UnauthorizedAccessException("Modifier user ID is missing."); // More specific error message

		//	// Collection Management for Nested Stages and Steps



		//	// Step 1: Manage Stages (Add/Update/Remove)

		//	var stagesToKeep = new List<CropStage>();

		//	foreach (var dtoStage in cropDto.Stages)
		//	{
		//		var existingStage = existingCrop.Stages.FirstOrDefault(s => s.Id == dtoStage.Id);
		//		if (existingStage == null || dtoStage.Id == 0) // New stage (not found by ID, or ID is 0)
		//		{
		//			existingStage = _mapper.Map<CropStage>(dtoStage); // Maps from CropStageDefinitionDto
		//			existingStage.CreatorId = currentUserId; // Set CreatorId for new stage
		//			existingStage.ActualCost = 0;
		//			existingStage.TotalActualCost = 0;
		//			existingStage.Id = 0; // Ensure ID is 0 for new entities for EF Core
		//			existingCrop.Stages.Add(existingStage);
		//		}
		//		else // Existing stage, update its properties
		//			_mapper.Map(dtoStage, existingStage); // Update scalar properties of existing stage

		//		stagesToKeep.Add(existingStage); // Add to a temporary list of stages we will keep

		//		// Step 2: Manage Steps within this (new or existing) stage

		//		var stepsToKeep = new List<Step>();
		//		foreach (var dtoStep in dtoStage.Steps)
		//		{
		//			var existingStep = existingStage.Steps.FirstOrDefault(st => st.Id == dtoStep.Id);
		//			if (existingStep == null || dtoStep.Id == 0) // New step
		//			{
		//				existingStep = _mapper.Map<Step>(dtoStep); // Maps from StepDefinitionDto
		//				existingStep.CreatorId = currentUserId; // Set CreatorId for new step
		//				existingStep.ActualCost = null;
		//				existingStep.ActualStartDate = null;
		//				existingStep.Id = 0; // Ensure ID is 0 for new entities
		//				existingStage.Steps.Add(existingStep); // Add to the parent stage's collection
		//			}
		//			else // Existing step, update its properties
		//				_mapper.Map(dtoStep, existingStep);

		//			stepsToKeep.Add(existingStep); // Add to temporary list of steps we will keep
		//		}

		//		// Mark steps for removal: any step in existingStage.Steps that is NOT in stepsToKeep
		//		var currentSteps = existingStage.Steps.ToList(); // Take a copy to modify original collection
		//		foreach (var stepToRemove in currentSteps)
		//		{
		//			if (!stepsToKeep.Contains(stepToRemove))
		//			{
		//				existingStage.Steps.Remove(stepToRemove);
		//				_unitOfWork.GetRepositories<Step, int>().SoftDelete(stepToRemove);
		//			}
		//		}

		//	}
		//	// Mark stages for removal: any stage in existingCrop.Stages that is NOT in stagesToKeep
		//	var currentStages = existingCrop.Stages.ToList();
		//	foreach (var stageToRemove in currentStages)
		//	{
		//		if (!stagesToKeep.Contains(stageToRemove))
		//		{
		//			existingCrop.Stages.Remove(stageToRemove);
		//			_unitOfWork.GetRepositories<CropStage, int>().SoftDelete(stageToRemove); // Mark for soft deletion
		//		}
		//	}

		//	RecalculateCropCosts(existingCrop);

		//	repo.Update(existingCrop);
		//	await _unitOfWork.SaveChangesAsync();
		//}


		public async Task UpdateCrops(CropDefinitionDto cropDto, string modifierUserId)
		{
			if (cropDto == null)
				throw new ArgumentNullException(nameof(cropDto), "Crop data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Crop, int>();
			// Load the existing entity from the database, including all its children.
			var spec = new CropSpecification(forUpdate: true, cropDto.Id);
			var existingCrop = await repo.GetByIdAWithSpecAsync(spec);

			if (existingCrop == null)
				throw new KeyNotFoundException($"Crop with ID {cropDto.Id} not found.");

			if (existingCrop.CreatorId != modifierUserId)
				throw new UnauthorizedAccessException("User is not authorized to update this crop plan.");

			// --- START OF THE NEW, FOOLPROOF LOGIC ---

			// 1. Manually update the primitive properties of the Crop.
			existingCrop.CropName = cropDto.CropName;
			existingCrop.PictureUrl = cropDto.PictureUrl;
			existingCrop.CropDescription = cropDto.CropDescription;
			existingCrop.StartDate = cropDto.StartDate;
			existingCrop.LastStartDate = cropDto.LastStartDate;
			existingCrop.Duration = cropDto.Duration;
			existingCrop.LandId = cropDto.LandId;

			// 2. Synchronize the Stages collection.
			var stagesInDtoById = cropDto.Stages?.ToDictionary(s => s.Id) ?? new Dictionary<int, StageDefinitionDto>();

			// Remove stages that are no longer present in the DTO.
			foreach (var stageInDb in existingCrop.Stages.ToList())
			{
				if (stageInDb.Id != 0 && !stagesInDtoById.ContainsKey(stageInDb.Id))
				{
					_unitOfWork.GetRepositories<CropStage, int>().SoftDelete(stageInDb);
				}
			}

			// Update existing stages and add new ones.
			if (cropDto.Stages != null)
			{
				foreach (var stageDto in cropDto.Stages)
				{
					var existingStage = existingCrop.Stages.FirstOrDefault(s => s.Id == stageDto.Id && s.Id != 0);

					if (existingStage != null)
					{
						// --- UPDATE EXISTING STAGE ---
						// Manually update the properties instead of using AutoMapper.
						existingStage.StageName = stageDto.StageName;
						existingStage.OptionalLink = stageDto.OptionalLink;
						existingStage.EstimatedCost = stageDto.EstimatedCost;

						// Synchronize the nested Steps for this stage.
						var stepsInDtoById = stageDto.Steps?.ToDictionary(st => st.Id) ?? new Dictionary<int, StepDefinitionDto>();
						foreach (var stepInDb in existingStage.Steps.ToList())
						{
							if (stepInDb.Id != 0 && !stepsInDtoById.ContainsKey(stepInDb.Id))
							{
								_unitOfWork.GetRepositories<Step, int>().SoftDelete(stepInDb);
							}
						}

						if (stageDto.Steps != null)
						{
							foreach (var stepDto in stageDto.Steps)
							{
								var existingStep = existingStage.Steps.FirstOrDefault(st => st.Id == stepDto.Id && st.Id != 0);
								if (existingStep != null)
								{
									// Manually update existing step properties.
									existingStep.StepName = stepDto.StepName;
									existingStep.Description = stepDto.Description;
									existingStep.Tool = stepDto.Tool;
									existingStep.ToolImage = stepDto.ToolImage;
									existingStep.DurationDays = stepDto.DurationDays;
									existingStep.Fertilizer = stepDto.Fertilizer;
									existingStep.EstimatedCost = stepDto.EstimatedCost;
									existingStep.PlannedStartDate = stepDto.PlannedStartDate;
								}
								else
								{
									// Add new step (mapping is safe here as it's a new object).
									var newStep = _mapper.Map<Step>(stepDto);
									newStep.CreatorId = modifierUserId;
									existingStage.Steps.Add(newStep);
								}
							}
						}
					}
					else
					{
						// --- ADD NEW STAGE ---
						// Mapping is safe here because it's a completely new entity graph.
						var newStage = _mapper.Map<CropStage>(stageDto);
						newStage.CreatorId = modifierUserId;
						if (newStage.Steps != null)
						{
							foreach (var step in newStage.Steps)
							{
								step.CreatorId = modifierUserId;
							}
						}
						existingCrop.Stages.Add(newStage);
					}
				}
			}

			// 3. Recalculate costs.
			RecalculateCropCosts(existingCrop);

			// 4. Save Changes.
			repo.Update(existingCrop);
			await _unitOfWork.SaveChangesAsync();
		}


	}
}
