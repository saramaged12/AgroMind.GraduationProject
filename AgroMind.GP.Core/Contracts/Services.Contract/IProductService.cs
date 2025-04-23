using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
	public interface IProductService
	{
		//Add Products
		Task AddAsync (ProductDTO productDto);

		//Get All Products
		Task<IReadOnlyList<ProductDTO>> GetAllProductsAsync();

		//Get By ID Products

		Task<ProductDTO> GetProductByIdAsync(int id);

		//Update Products
		
		void UpdateProducs(ProductDTO productDTO);

		//Delete Products
		void DeleteProducts(ProductDTO productDTO);



		



	}
}
