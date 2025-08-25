using AgroMind.GP.APIs.DTOs;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IStageService
	{
		Task<CropStageDto> AddStageAsync(StageDefinitionDto stageDto);

		Task<IReadOnlyList<CropStageDto>> GetAllStagesAsync();



		Task<CropStageDto> GetStageByIdAsync(int id);



		//Task UpdateStage(CropStageDto stageDto);


		Task DeleteStage(int ID);

		Task<IReadOnlyList<CropStageDto>> GetAllDeletedStagesAsync();
	}
}
