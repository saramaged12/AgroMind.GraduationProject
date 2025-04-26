using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IStageService
	{
		Task<CropStageDto> AddStageAsync(CropStageDto stageDto);

		Task<IReadOnlyList<CropStageDto>> GetAllStagesAsync();



		Task<CropStageDto> GetStageByIdAsync(int id);



		Task UpdateStage(CropStageDto stageDto);


		Task DeleteStage(CropStageDto stageDto);
	}
}
