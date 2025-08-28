using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Orders
{
	public class Address 
	{
		//Address of Order
		//Adddress is owned Entity By Order (Not Have PK) ,
		//Owner is Order Entity
		public required string FirstName { get; set; }

		public required string LastName { get; set; }

		public required string City { get; set; }

		public required string Street { get; set; }

		public required string Country { get; set; }
	}
}
