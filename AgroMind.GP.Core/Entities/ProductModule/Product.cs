using AgroMind.GP.Core.Entities.Identity;

namespace AgroMind.GP.Core.Entities.ProductModule
{
	public class Product : BaseEntity<int>
	{

		public string? Name { get; set; }

		public string? Description { get; set; }

		public string? PictureUrl { get; set; }

		public decimal Price { get; set; }

		public int? CategoryId { get; set; } //FK

		public Category? Category { get; set; } // 1 Category to M Product

		public int? BrandId { get; set; } //FK 
		public Brand? Brand { get; set; } // 1 Brand to M Producr

		public Supplier? Supplier { get; set; } //1
		public string? SupplierId { get; set; }

	}
}
