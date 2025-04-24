using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IBrandService
	{
		//Add Brands

	    Task<BrandDTO> AddBrandAsync(BrandDTO brandDto);

		//Get All Brandss
		Task<IReadOnlyList<BrandDTO>> GetAllBrandsAsync();

		//Get By ID Brands

		Task<BrandDTO> GetBrandsByIdAsync(int id);

		//Update Brandss

		Task UpdateBrands(BrandDTO brandDTO);

		//Delete Categories
		Task DeleteBrands(BrandDTO brandDTO);
	}
}
