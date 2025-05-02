using AgroMind.GP.Core.Contracts;

namespace AgroMind.GP.Core.Entities
{
	public class BaseEntity<TKey>:ISoftDelete
	{
		public TKey Id { get; set; }

		public bool IsDeleted { get; set; } = false; // Default to not deleted
		public DateTime? DeletedAt { get; set; } // Nullable for non-deleted entities

	}
}
