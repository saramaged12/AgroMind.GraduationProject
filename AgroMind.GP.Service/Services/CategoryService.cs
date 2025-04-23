using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CategoryService(IMapper mapper , IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task AddCategoryAsync(CategoryDTO categoryDto)
		{
			// Maps the DTO to the Category entity and adds it to the database
			var repo = _unitOfWork.GetRepositories<Category, int>();
			var categoryEntity = _mapper.Map<Category>(categoryDto);
			await repo.AddAsync(categoryEntity);

		}

		public void DeleteCategories(CategoryDTO categoryDto)
		{
			var repo = _unitOfWork.GetRepositories<Category, int>();
			var categoryEntity = _mapper.Map<Category>(categoryDto);
			repo.Delete(categoryEntity);

		}

		public async Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync()
		{
		   var categories=await _unitOfWork.GetRepositories<Category, int>() .GetAllAsync();
		  
		   return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
		  
		}

		public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
		{
			var category= await _unitOfWork.GetRepositories<Category, int>().GetByIdAsync(id);
			var categorydto=_mapper.Map<Category,CategoryDTO>(category);
			return categorydto;
		}

		public void UpdateCategories(CategoryDTO categoryDto)
		{
			// Maps the DTO to the Category entity and updates it
			var repo = _unitOfWork.GetRepositories<Category, int>();
			var categoryEntity = _mapper.Map<Category>(categoryDto);
			repo.Update(categoryEntity);

		}
	}
}
