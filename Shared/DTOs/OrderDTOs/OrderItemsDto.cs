using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDTOs
{
	public class OrderItemsDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }

		public required string ProductName { get; set; }

		public required string PictureUrl { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}
