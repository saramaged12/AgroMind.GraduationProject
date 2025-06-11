using AgroMind.GP.Core.Contracts;
using AgroMind.GP.Core.Entities.Identity;

namespace AgroMind.GP.Core.Entities
{
	public class BaseEntity<TKey>:ISoftDelete
	{
		public TKey Id { get; set; }

		public bool IsDeleted { get; set; } = false; // Default to not deleted
		public DateTime? DeletedAt { get; set; } 

		public string? CreatorId { get; set; } 
		public AppUser? Creator { get; set; } // the AppUser who created this

		

	}
}
