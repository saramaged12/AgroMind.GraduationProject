using AgroMind.GP.APIs.DTOs;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface ILandService
	{
		
		Task<LandDTO> AddAsync(LandDTO landDto);

	
		Task<IReadOnlyList<LandDTO>> GetAllLandsAsync();

	

		Task<LandDTO> GetLandByIdAsync(int id);

		

		Task UpdateLands(LandDTO landDTO);

		Task DeleteLands(LandDTO landDTO);
	}
}
