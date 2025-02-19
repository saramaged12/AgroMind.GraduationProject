using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Identity
{
	public class AppUser : IdentityUser
	{

		public string FName { get; set; }
		public string LName {  get; set; }

		public string Gender { get; set; }
		public int Age {get; set; }

		public Address Address { get; set; }

		public string Role {  get; set; }

		public bool IsActive { get; set; }
		public bool IsBlocked { get; set; }
		public DateTime? LastLogin { get; set; }


	}
}
