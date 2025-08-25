using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
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
					throw new NotFoundException(nameof(Brand), id);

				return _mapper.Map<Brand, BrandDTO>(brand);
			}

			public async Task UpdateBrands(BrandDTO brandDto)
			{

				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var existingBrand = await repo.GetByIdAsync(brandDto.Id);
				
			    _mapper.Map(brandDto, existingBrand);
			    repo.Update(existingBrand);
			    await _unitOfWork.SaveChangesAsync();
		    }

			public async Task DeleteBrands(int id)
			{
				
				var repo = _unitOfWork.GetRepositories<Brand, int>();
				var existingBrand = await repo.GetByIdAsync(id);

			    if (existingBrand == null)
				throw new NotFoundException(nameof(Brand), id);

			    repo.SoftDelete(existingBrand);
				await _unitOfWork.SaveChangesAsync();
			}

		    public async Task<IReadOnlyList<BrandDTO>> GetAllDeletedBrandsAsync()
		    {
			var repo = _unitOfWork.GetRepositories<Brand, int>();
			var deletedbrands = await repo.GetAllDeletedAsync();
			return _mapper.Map<IReadOnlyList<BrandDTO>>(deletedbrands);
		    }
	}
	}
	

