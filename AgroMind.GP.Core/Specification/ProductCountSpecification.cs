using AgroMind.GP.Core.Entities.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class ProductCountSpecification : BaseSpecifications<Product, int>
	{
		public ProductCountSpecification(ProductQueryParams queryParams) : base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId)
		&& (!queryParams.CategoryId.HasValue || p.CategoryId == queryParams.CategoryId) && (!p.IsDeleted)
			&& (string.IsNullOrWhiteSpace(queryParams.SearchValue) || (p.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))))

		{
		}
	}
}
