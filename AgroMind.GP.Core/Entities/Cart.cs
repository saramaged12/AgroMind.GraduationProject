using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Cart
	{
		//public string FarmerId { get; set; } // Foreign Key to farmer
		public string Id { get; set; } //Guid

        public List<CartItem> Items { get; set; } = new List<CartItem>();

		public Cart(string id)
		{
			
			Id = id;

		}

	}
}
