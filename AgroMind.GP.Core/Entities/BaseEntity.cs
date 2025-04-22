namespace AgroMind.GP.Core.Entities
{
	public class BaseEntity<TKey>
	{
		public TKey Id { get; set; }
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; }

	}
}
