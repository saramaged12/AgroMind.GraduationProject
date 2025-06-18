using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class CartDto
	{
		public string Id { get; set; } // Cart ID / User ID
		public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
		public decimal TotalPrice { get; set; } // Calculated total
	}
}
