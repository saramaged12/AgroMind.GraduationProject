using AgroMind.GP.APIs.DTOs;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IStepService
	{
		Task<StepDto> AddStepAsync(StepDefinitionDto stepDto , string creatorUserId);

		Task<IReadOnlyList<StepDto>> GetAllStepsAsync();



		Task<StepDto> GetStepByIdAsync(int id);



		//Task UpdateStep(StepDto stepDto);


		Task DeleteStep(StepDto stepDto);

		Task<IReadOnlyList<StepDto>> GetAllDeletedStepsAsync();
	}
}
