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
		public class BrandService : IBrandService
		{
			private readonly IUnitOfWork _unitOfWork;
			private readonly IMapper _mapper;

			public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
			{
				_unitOfWork = unitOfWork;
				_mapper = mapper;
			}

			public async Task<BrandDTO> AddBrandAsync(BrandDTO brandDto)
			{
				if (brandDto == null)
					throw new ArgumentNullException(nameof(brandDto), "Brand data cannot be null.");

			    var brandEntity = _mapper.Map<Brand>(brandDto);
			    var repo = _unitOfWork.GetRepositories<Brand, int>();
			
				await repo.AddAsync(brandEntity);
				await _unitOfWork.SaveChangesAsync();
			    return _mapper.Map<BrandDTO>(brandEntity);
			}

			public async Task<IReadOnlyList<BrandDTO>> GetAllBrandsAsync()
			{
				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var brands = await repo.GetAllAsync();
				return _mapper.Map<IReadOnlyList<Brand>, IReadOnlyList<BrandDTO>>(brands);
			}

			public async Task<BrandDTO> GetBrandsByIdAsync(int id)
			{
				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var brand = await repo.GetByIdAsync(id);

				if (brand == null)
					throw new KeyNotFoundException($"Brand with ID {id} not found.");

				return _mapper.Map<Brand, BrandDTO>(brand);
			}

			public async Task UpdateBrands(BrandDTO brandDto)
			{
				if (brandDto == null)
					throw new ArgumentNullException(nameof(brandDto), "Brand data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var existingBrand = await repo.GetByIdAsync(brandDto.Id);

				if (existingBrand == null)
					throw new KeyNotFoundException($"Brand with ID {brandDto.Id} not found.");

			    // Map the updated properties to the existing entity
			       _mapper.Map(brandDto, existingBrand);


			    // Update the existing entity
			       repo.Update(existingBrand);
			       await _unitOfWork.SaveChangesAsync();
		    }

			public async Task DeleteBrands(BrandDTO brandDto)
			{
				if (brandDto == null)
					throw new ArgumentNullException(nameof(brandDto), "Brand data cannot be null.");

				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var existingBrand = await repo.GetByIdAsync(brandDto.Id);

				if (existingBrand == null)
					throw new KeyNotFoundException($"Brand with ID {brandDto.Id} not found.");

				repo.Delete(existingBrand);
				await _unitOfWork.SaveChangesAsync();
			}
		}
	}
	

