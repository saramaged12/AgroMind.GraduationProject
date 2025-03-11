using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class CropStage : BaseEntity<int>
	{

		public string Stage { get; set; } //name

		public int? Duration { get; set; } // Number of days

		public string? Status { get; set; } // "pending" or "completed"
		[JsonIgnore]
		public Crop Crop { get; set; }
		public int? CropId { get; set; } // Foreign key reference

		public static implicit operator string(CropStage v)
		{
			throw new NotImplementedException();
		}
	}
}
