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

		public async Task AddBrandAsync(BrandDTO brandDto)
		{
			// Maps the DTO to the Brand entity and adds it to the database
			var repo = _unitOfWork.GetRepositories<Brand, int>();
			var brandEntity = _mapper.Map<Brand>(brandDto);
			await repo.AddAsync(brandEntity);

		}

		public async Task<IReadOnlyList<BrandDTO>> GetAllBrandsAsync()
		{
			var repo=_unitOfWork.GetRepositories<Brand, int>();
			var brands=await repo.GetAllAsync();
			var brandsDTO = _mapper.Map<IReadOnlyList<Brand>, IReadOnlyList<BrandDTO>>(brands);
			return brandsDTO;
		}

		public async Task<BrandDTO> GetBrandsByIdAsync(int id)
		{

			// Retrieves a brand by ID and maps it to a DTO
			var brand = await _unitOfWork.GetRepositories<Brand, int>().GetByIdAsync(id);
			return _mapper.Map<Brand, BrandDTO>(brand);

		}

		public void UpdateBrandss(BrandDTO brandDto)
		{
			var repo = _unitOfWork.GetRepositories<Brand, int>();
			var brandEntity = _mapper.Map<Brand>(brandDto);
			repo.Update(brandEntity);

		}

		public void DeleteBrandss(BrandDTO brandDto)
		{
			var repo = _unitOfWork.GetRepositories<Brand, int>();
			var brandEntity = _mapper.Map<Brand>(brandDto);
			repo.Delete(brandEntity);

		}

	}
}