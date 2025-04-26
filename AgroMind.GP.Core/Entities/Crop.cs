using AgroMind.GP.Core.Entities.Identity;
using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class Crop : BaseEntity<int>
	{

		public string? CropName { get; set; }

		public string? PictureUrl { get; set; } // Nullable to match frontend `cropImage`
		public string? CropDescription { get; set; } // Changed from "Description" to avoid conflict

		public List<CropStage> Stages { get; set; } = new List<CropStage>(); // Changed to List<>

		
		// Null-safe calculated property
		public decimal TotalCost => Stages?.Sum(stage => stage.TotalCost) ?? 0;

		//TotalCost is a calculated property that dynamically computes the total cost of all stages in the Stages list.

		//This ensures that the value of TotalCost is always up-to-date and reflects the current state of the Stages.


		public string? FarmerId { get; set; }
		[JsonIgnore] // Prevent infinite loop during serialization
		public Farmer? Farmer { get; set; }

		public int? LandId { get; set; }
		public Land? Land { get; set; }

		
	
		///Remove?
		//public string? CropType { get; set; }

		//public DateTime? plantingDate { get; set; }//list who the crop that will  //remove this add start and end date

		//public string? LandPlantedType { get; set; }

		//public string? AreaPlanted { get; set; }

		//public string? CropHealthStatus { get; set; }// remove

		


	}
}
