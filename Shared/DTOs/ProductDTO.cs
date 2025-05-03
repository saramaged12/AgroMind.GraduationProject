
namespace AgroMind.GP.APIs.DTOs
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public string? Name { get; set; }

		public string? Description { get; set; }

		public string? PictureUrl { get; set; }

		public decimal Price { get; set; }
    
		public int? CategoryId { get; set; } //FK

		public int? BrandId { get; set; } //FK 

		public string? CategoryName { get; set; } //FK 
		public string? BrandName { get; set; } // 1 Brand to M Producr

		//public string? SupplierName { get; set; } //1
		
	}

}
