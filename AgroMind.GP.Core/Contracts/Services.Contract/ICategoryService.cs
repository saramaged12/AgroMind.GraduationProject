using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface ICategoryService
	{
		//Add Categories

		Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto);
		//Get All Categories
		Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync();

		//Get By ID Categories

		Task<CategoryDTO> GetCategoryByIdAsync(int id);

		//Update Categoriess

		Task UpdateCategories(CategoryDTO categorydto);

		//Delete Categories
		Task DeleteCategories(CategoryDTO categoryDTO);
	}
}
