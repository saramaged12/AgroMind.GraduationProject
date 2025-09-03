using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDTOs
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public required string BuyerEmail { get; set; }

		public DateTimeOffset OrderDate { get; set; }
		public required string Status { get; set; }

		public required AddressDto ShippingAddress { get; set; }
		public int? DeliveryMethodId { get; set; } //1 to Many
		public string? DeliveryMethod { get; set; }

		public virtual required ICollection<OrderItemsDto> OrderItems { get; set; }
		public decimal Subtotal { get; set; }  //Cost Of Order without Delivery

		public decimal Total { get; set; }

	}
}
