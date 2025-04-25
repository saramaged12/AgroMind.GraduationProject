using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface ICropService
	{
		Task<CropDto> AddCropAsync(CropDto cropDto);
		
		Task<IReadOnlyList<CropDto>> GetAllCropsAsync();

		

		Task<CropDto> GetCropByIdAsync(int id);

		

		Task UpdateCrops(CropDto cropDto);

		
		Task DeleteCrop(CropDto cropDto);
	}
}
