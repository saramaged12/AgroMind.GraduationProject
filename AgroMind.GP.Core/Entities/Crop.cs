using AgroMind.GP.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{


	public class Crop : BaseEntity<int>
	{

		public string? CropName { get; set; }

		public string? PictureUrl { get; set; } 
		public string? CropDescription { get; set; } 

		public List<CropStage>? Stages { get; set; } = new List<CropStage>(); 

		
	
		public decimal? TotalEstimatedCost { get; set; }

		public decimal? TotalActualCost { get; set; }



		public int? LandId { get; set; }
		public Land? Land { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime LastStartDate { get; set; }

		public int Duration { get; set; }

		public CropPlanType? PlanType {  get; set; }

	

	}

	
}
