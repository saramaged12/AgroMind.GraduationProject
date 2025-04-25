using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class CropStage : BaseEntity<int>
	{

		public string? StageName { get; set; } 

		public string? OptionalLink { get; set; }

		public List<Step> Steps { get; set; } = new List<Step>(); 

		public decimal TotalCost { get; set; } 

		public int? CropId { get; set; }
		[JsonIgnore] // Prevent infinite loop
		public Crop? Crop { get; set; }

		
		

	}
}
