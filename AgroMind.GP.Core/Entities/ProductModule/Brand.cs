namespace AgroMind.GP.Core.Entities.ProductModule
{
	public class Brand : BaseEntity<int>
	{
		//public int Id { get; set; }

		public string? BrandName { get; set; }

		public string? PictureUrl { get; set; }

		//public ICollection<Product> Products { get; set; } = new List<Product>(); //M
	}
}
