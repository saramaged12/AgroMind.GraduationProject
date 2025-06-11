using AgroMind.GP.APIs.DTOs;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface ICropService
	{
		Task<CropDto> AddCropAsync(CropDefinitionDto cropDto, string creatorUserId, string creatorRole);

		Task<CropDto> AdoptRecommendedCropAsync(int recommendedCropId, string farmerUserId);

		Task UpdateCrops(CropDefinitionDto cropDto, string modifierUserId);


		// UPDATE (Actuals - comprehensive update for ALL estimated/actuals)
		Task UpdateActualsForCropAsync(int cropId, CropDto cropDtoWithActuals, string modifierUserId); // Takes full CropDto
		
		Task DeleteCrop(CropDto cropDto);

	
		Task<IReadOnlyList<CropDto>> GetAllDeletedCropsAsync();
		Task<IReadOnlyList<CropDto>> GetRecommendedCropsAsync(RecommendRequestDTO recommendDto);


		Task<PlanInfoDto> GetPlanAndCreatorInfoAsync(int cropId);

		Task<IReadOnlyList<PlanInfoDto>> GetAllPlansWithCreatorInfoAsync();
		//// Actuals update uses specific DTO
		//Task RecordStepCompletionAsync(int stepId, StepCompletionDto completionDto, string farmerUserId);
		Task<IReadOnlyList<CropDto>> GetMyPlansAsync(string farmerUserId);

		Task<IReadOnlyList<CropDto>> GetAllCropsAsync();
		Task<CropDto> GetCropByIdAsync(int id);

	}

}
