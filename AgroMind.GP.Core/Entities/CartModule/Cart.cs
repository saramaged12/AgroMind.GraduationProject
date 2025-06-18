namespace AgroMind.GP.Core.Entities
{
	public class Cart
	{
		
		public string Id { get; set; }  // will be the Redis Key ( Farmer.Id or a GUID for anonymous)
		public List<CartItem> Items { get; set; } = new List<CartItem>();

		

		

		// Constructor for a new cart
		public Cart(string id)
		{

			Id = id;

		}

		// parameterless constructor for deserialization
		public Cart()
		{

		}


	}
}
