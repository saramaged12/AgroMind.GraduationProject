using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class CropStage : BaseEntity<int>
	{

		public string? StageName { get; set; } 

		public string? OptionalLink { get; set; }

		public List<Step>? Steps { get; set; } = new List<Step>(); 

		public decimal? EstimatedCost { get; set; } // Cost of the stage it self

		public decimal? ActualCost { get; set; }

		public decimal? TotalEstimatedCost { get; set; } //  The sum of the stage's cost and the costs of all its steps.

		public decimal ?TotalActualCost { get; set; }

		public int? CropId { get; set; }

		[JsonIgnore] // Prevent infinite loop
		public Crop? Crop { get; set; }

		
		

	}
}
