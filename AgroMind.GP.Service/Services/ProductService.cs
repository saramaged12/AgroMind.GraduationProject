using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AgroMind.GP.Service.Services
{
        // The controller should focus on handling HTTP requests and responses
		//, while the service layer should handle business logic, including validation and null checks.
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		//Unit of Work -> If I will use the repository pattern, I will use the unit of work to manage the repositories
		//if i have than more one REpo
		public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ProductDTO> AddAsync(ProductDTO productDto)
		{
			if (productDto == null)
				throw new ArgumentNullException(nameof(productDto));

			var productEntity = _mapper.Map<Product>(productDto);
			var repo = _unitOfWork.GetRepositories<Product, int>();

			await repo.AddAsync(productEntity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ProductDTO>(productEntity);
		}
		public async Task DeleteProducts(ProductDTO productDto)
		{
			if (productDto == null)
				throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Product, int>();
			var productEntity = await repo.GetByIdAsync(productDto.Id);
			if (productEntity == null)
				throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");

			repo.Delete(productEntity);
			await _unitOfWork.SaveChangesAsync();


		}

		public async Task<IReadOnlyList<ProductDTO>> GetAllProductsAsync()
		{
			var Specification = new ProductWithBrandAndCategorySpec();
			var Repo =_unitOfWork.GetRepositories<Product, int>();
			var Products= await Repo.GetAllWithSpecASync(Specification);
			var ProducsDTO = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDTO>>(Products);
			return ProducsDTO;
			
		}

		public async Task<ProductDTO> GetProductByIdAsync(int id)
		{
			var Specifications= new ProductWithBrandAndCategorySpec(id);	
			var product= await _unitOfWork.GetRepositories<Product, int>().GetByIdAWithSpecAsync(Specifications);
			if (product == null)
				throw new KeyNotFoundException($"Product with ID {id} not found.");

			return _mapper.Map<Product, ProductDTO>(product);	
		}

		public async Task UpdateProducs(ProductDTO productDto)
		{
			//Maps the DTO to the entity and updates the product.
			if (productDto == null)
				throw new ArgumentNullException(nameof(productDto), "Product data cannot be null.");

			var repo = _unitOfWork.GetRepositories<Product, int>();

			var existingProduct =  await repo.GetByIdAsync(productDto.Id);
			if (existingProduct == null)
				throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");

			// Map the updated properties to the existing entity
			_mapper.Map(productDto, existingProduct);


			// Update the existing entity
			repo.Update(existingProduct);
			await _unitOfWork.SaveChangesAsync();



		}
	}
}
