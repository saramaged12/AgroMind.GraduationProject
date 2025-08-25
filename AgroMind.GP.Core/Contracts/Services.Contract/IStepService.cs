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
		Task<StepDto> AddStepAsync(StepDefinitionDto stepDto );

		Task<IReadOnlyList<StepDto>> GetAllStepsAsync();



		Task<StepDto> GetStepByIdAsync(int id);



		//Task UpdateStep(StepDto stepDto);


		Task DeleteStep(int id);

		Task<IReadOnlyList<StepDto>> GetAllDeletedStepsAsync();
	}
}
