using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class AddressDTO
	{
		public string Id { get; set; } 
		public string? Fname { get; set; } // who will receive order

		public string? Lname { get; set; }

		public string? city { get; set; }

		public string? street { get; set; }

		public string? country { get; set; }


	}
}
