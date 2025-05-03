using AgroMind.GP.Core.Entities.ProductModule;
using Shared;

namespace AgroMind.GP.Core.Specification
{
	public class ProductWithBrandAndCategorySpec : BaseSpecifications<Product, int>
	{
		//For Get All Products
		public ProductWithBrandAndCategorySpec(int? BrandId, int? CategoryId,ProductSortingOptions sortingOptions) 
			: base(p=>(!BrandId.HasValue||p.BrandId==BrandId)
		&&(!CategoryId.HasValue||p.CategoryId==CategoryId) && !p.IsDeleted)
		// Filters products by BrandId and CategoryId if provided, otherwise includes all.

		{
		
			AddInclude(p => p.Brand);
			AddInclude(p => p.Category);
			//Includes.Add(p => p.Supplier);

			switch (sortingOptions)
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
