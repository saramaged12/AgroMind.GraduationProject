using AgroMind.GP.Core.Contracts.Common;
using AgroMind.GP.Core.Entities.Identity;

namespace AgroMind.GP.Core.Entities
{
    public abstract class BaseEntity<TKey>:ISoftDelete
	{
		public TKey Id { get; set; }
		public DateTime CreatedAt { get; set; } 
		public string? CreatedBy { get; set; } 
		public DateTime? LastModifiedAt { get; set; } 
		public string? LastModifiedBy { get; set; } 
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; }


	}
}
