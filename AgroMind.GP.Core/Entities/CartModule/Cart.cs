namespace AgroMind.GP.Core.Entities
{
	public class Cart
	{
		
		public required string Id { get; set; }  // will be the Redis Key ( Farmer.Id or a GUID for anonymous)
		public required  IEnumerable<CartItem> Items { get; set; } 

	


	}
}
