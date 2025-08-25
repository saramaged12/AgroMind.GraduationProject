using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Exceptions;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AgroMind.GP.Service.Services
{
      
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

	
		public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ProductDTO> AddAsync(ProductDTO productDto)
		{
			
			var productEntity = _mapper.Map<Product>(productDto);
			var repo = _unitOfWork.GetRepositories<Product, int>();

			await repo.AddAsync(productEntity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ProductDTO>(productEntity);
		}
		public async Task DeleteProducts(int Id)
		{
			
			var repo = _unitOfWork.GetRepositories<Product, int>();
			var productEntity = await repo.GetByIdAsync(Id);

			if (productEntity == null)
				throw new NotFoundException(nameof(Product),Id);

			repo.SoftDelete(productEntity);
			await _unitOfWork.SaveChangesAsync();

		}


		public async Task<PaginatedResultDTO<ProductDTO>>GetAllProductsAsync(ProductQueryParams queryParams)
		{
			var Specification = new ProductWithBrandAndCategorySpec(queryParams);
			var Repo =_unitOfWork.GetRepositories<Product, int>();
			var Products= await Repo.GetAllWithSpecASync(Specification);
			var Data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDTO>>(Products);
			var ProductCount=Data.Count();
			var CountSpec = new ProductCountSpecification(queryParams);
			var TotalCount= await Repo.CountAsync(CountSpec);
			return new PaginatedResultDTO<ProductDTO>(ProductCount,queryParams.PageIndex,TotalCount,Data);
			
		}

		public async Task<ProductDTO> GetProductByIdAsync(int id)
		{
			var Specifications= new ProductWithBrandAndCategorySpec(id);	
			var product= await _unitOfWork.GetRepositories<Product, int>().GetByIdAWithSpecAsync(Specifications);
			if (product == null)
				throw new NotFoundException(nameof(Product), id);

			return _mapper.Map<Product, ProductDTO>(product);	
		}

		public async Task UpdateProducs(ProductDTO productDto)
		{
			
			var repo = _unitOfWork.GetRepositories<Product, int>();

			var existingProduct =  await repo.GetByIdAsync(productDto.Id);
			
			_mapper.Map(productDto, existingProduct);

			repo.Update(existingProduct);
			await _unitOfWork.SaveChangesAsync();


		}

		
		public async Task<IReadOnlyList<ProductDTO>> GetAllDeletedProductsAsync()
		{
			var repo = _unitOfWork.GetRepositories<Product, int>();
			var deletedProducts = await repo.GetAllDeletedAsync(); 
			return _mapper.Map<IReadOnlyList<ProductDTO>>(deletedProducts);
		}

	
	}
}
