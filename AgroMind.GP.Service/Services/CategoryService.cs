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

		    public CategoryService( IUnitOfWork unitOfWork, IMapper mapper)
		    {
			   _mapper = mapper;
			   _unitOfWork = unitOfWork;
		    }

		    public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto)
			{
				if (categoryDto == null)
					throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");

			var categoryEntity = _mapper.Map<Category>(categoryDto);
			var repo = _unitOfWork.GetRepositories<Category, int>();
			
				await repo.AddAsync(categoryEntity);
				await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CategoryDTO>(categoryEntity);
			}

			public async Task DeleteCategories(CategoryDTO categoryDto)
			{
				if (categoryDto == null)
					throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Category, int>();
				var existingCategory = await repo.GetByIdAsync(categoryDto.Id);

				if (existingCategory == null)
					throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");

				repo.Delete(existingCategory);
				await _unitOfWork.SaveChangesAsync();
			}

			public async Task<IReadOnlyList<CategoryDTO>> GetAllCategoriesAsync()
			{
				var repo = _unitOfWork.GetRepositories<Category, int>();
				var categories = await repo.GetAllAsync();
				return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
			}

			public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
			{
				var repo = _unitOfWork.GetRepositories<Category, int>();
				var category = await repo.GetByIdAsync(id);

				if (category == null)
					throw new KeyNotFoundException($"Category with ID {id} not found.");

				return _mapper.Map<Category, CategoryDTO>(category);
			}

			public async Task UpdateCategories(CategoryDTO categoryDto)
			{
				if (categoryDto == null)
					throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Category, int>();
				var existingCategory = await repo.GetByIdAsync(categoryDto.Id);

				if (existingCategory == null)
					throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");

				

		       	// Map the updated properties to the existing entity
			      _mapper.Map(categoryDto, existingCategory);

		
			   // Update the existing entity
			       repo.Update(existingCategory);
			       await _unitOfWork.SaveChangesAsync();

		}
	}
	}

