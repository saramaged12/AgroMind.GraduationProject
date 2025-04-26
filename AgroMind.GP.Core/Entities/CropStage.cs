using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class CropStage : BaseEntity<int>
	{

		public string? StageName { get; set; } 

		public string? OptionalLink { get; set; }

		public List<Step> Steps { get; set; } = new List<Step>(); 

		public decimal Cost { get; set; } // Cost of the stage it self

		public decimal TotalCost { get; set; } //  The sum of the stage's cost and the costs of all its steps.
											   //  Cost of the stage + sum of all step costs


		public int? CropId { get; set; }
		[JsonIgnore] // Prevent infinite loop
		public Crop? Crop { get; set; }

		
		

	}
}
