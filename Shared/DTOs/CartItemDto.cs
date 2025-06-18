using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class CartItemDto
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string? ProductName { get; set; } 
		public string? PictureUrl { get; set; } 
		public string? BrandName { get; set; }
		public string? CategoryName { get; set; } 
		public decimal Price { get; set; } // Current price from Product (or PriceAtAddition)
		public decimal LinePrice { get; set; } // Quantity * Price
	}
}
