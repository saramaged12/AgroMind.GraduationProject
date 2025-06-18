namespace AgroMind.GP.Core.Entities
{
	public class CartItem
	{
		public int ProductId { get; set; }

		public int Quantity { get; set; }

		
		public string? PictureUrlAtAddition { get; set; }   // Picture URL when item was added/updated


		public decimal PriceAtAddition { get; set; } // Price when item was added/updated

	}
}
