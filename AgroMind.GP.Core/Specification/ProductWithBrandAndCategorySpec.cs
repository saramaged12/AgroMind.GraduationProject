using AgroMind.GP.Core.Entities.ProductModule;
using Shared;

namespace AgroMind.GP.Core.Specification
{
	public class ProductWithBrandAndCategorySpec : BaseSpecifications<Product, int>
	{
		//For Get All Products
		public ProductWithBrandAndCategorySpec(ProductQueryParams queryParams)
			: base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId)
		&& (!queryParams.CategoryId.HasValue || p.CategoryId == queryParams.CategoryId) && (!p.IsDeleted)
			&& (string.IsNullOrWhiteSpace(queryParams.SearchValue) || (p.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))))
			
		// Filters products by BrandId and CategoryId if provided, otherwise includes all.

		{
		
			AddInclude(p => p.Brand);
			AddInclude(p => p.Category);
			//Includes.Add(p => p.Supplier);

			switch (queryParams. sortingOptions)
			{
				case ProductSortingOptions.NameAscending:
					AddOrderBy(P=>P.Name);
					break;
				case ProductSortingOptions.NameDescending:
					AddOrderByDescending(P => P.Name);
					break;
				case ProductSortingOptions.PriceAscending:
					AddOrderBy(P => P.Price);
					break;

				case ProductSortingOptions.PriceDescending:
					AddOrderByDescending(P => P.Price);
					break;
				default:
					break;
			}
			
			
		}

		//Get Product By Id
		public ProductWithBrandAndCategorySpec(int id) : base(p => p.Id == id && !p.IsDeleted)
		{
		    AddInclude(p => p.Brand);
			AddInclude(p => p.Category);
			//Includes.Add(p => p.Supplier);
		}
	}
}
