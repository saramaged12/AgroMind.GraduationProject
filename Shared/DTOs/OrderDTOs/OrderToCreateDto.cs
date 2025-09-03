using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderDTOs
{
	public class OrderToCreateDto
	{
		public  required string CartId { get; set; } 
		public int DeliveryMethodId { get; set; }

		public required AddressDto ShipToAddress { get; set; }
	}
}
