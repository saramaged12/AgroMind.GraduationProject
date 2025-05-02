using AgroMind.GP.Core.Contracts;
using Microsoft.AspNetCore.Identity;

namespace AgroMind.GP.Core.Entities.Identity
{
	public class AppUser : IdentityUser,ISoftDelete
	{

		public string FName { get; set; }
		public string LName { get; set; }

		public string Gender { get; set; }
		public int Age { get; set; }

		public Address? Address { get; set; }

		// Soft-delete properties
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; }


	}
}
