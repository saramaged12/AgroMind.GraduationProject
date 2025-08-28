using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Orders
{
	public class ProductItemOrdered 
	{
		//Owned By OrderItems 
		public int ProductId { get; set; }

		public required string ProductName { get; set; }

		public required string PictureUrl { get; set; }
	}
}
