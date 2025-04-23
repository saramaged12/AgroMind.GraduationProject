using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Contracts.Repositories.Contract;
using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Contracts.UnitOfWork.Contract;
using AgroMind.GP.Core.Entities.ProductModule;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Service.Services
{
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

		public async Task AddAsync(ProductDTO productDto)
		{
			//Takes a ProductDTO, maps it to the real Product entity, adds it to the DB.
			var repo =_unitOfWork.GetRepositories<Product, int>();
		    var productdto=_mapper.Map<Product>(productDto);
		    await repo.AddAsync(productdto);
			


		}

		public void DeleteProducts(ProductDTO productDTO)
		{
			//Maps the DTO to Product entity and deletes it.
			var repo = _unitOfWork.GetRepositories<Product, int>();
			var productEntity = _mapper.Map<Product>(productDTO);
			repo.Delete(productEntity);
			

		}

		public async Task<IReadOnlyList<ProductDTO>> GetAllProductsAsync()
		{
			var Repo =_unitOfWork.GetRepositories<Product, int>();
			var Products= await Repo.GetAllAsync();
			var ProducsDTO = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDTO>>(Products);
			return ProducsDTO;
			
		}

		public async Task<ProductDTO> GetProductByIdAsync(int id)
		{
			var product= await _unitOfWork.GetRepositories<Product, int>().GetByIdAsync(id);
			return _mapper.Map<Product, ProductDTO>(product);	
		}

		public void UpdateProducs(ProductDTO productDTO)
		{
			//Maps the DTO to the entity and updates the product.
			var repo = _unitOfWork.GetRepositories<Product, int>();
			var productEntity = _mapper.Map<Product>(productDTO);
			repo.Update(productEntity);
			

		}
	}
}
