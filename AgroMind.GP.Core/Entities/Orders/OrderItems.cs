using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Orders
{
	public class OrderItems : BaseEntity<int>
	{
		

		public required ProductItemOrdered Product { get; set; } 

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}
