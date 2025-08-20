namespace AgroMind.GP.Core.Entities
{
	public class CartItem
	{
		public int Id { get; set; } //Product

		public required string ProductName { get; set; }

		public int Quantity { get; set; }

		
		public string? PictureUrl { get; set; }  


		public decimal Price { get; set; } 

		public string? Brand { get; set; }

		public string? Category { get; set; }

	}
}
