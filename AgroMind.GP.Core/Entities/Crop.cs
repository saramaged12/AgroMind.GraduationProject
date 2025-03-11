using AgroMind.GP.Core.Entities.Identity;
using System.Text.Json.Serialization;

namespace AgroMind.GP.Core.Entities
{
	public class Crop : BaseEntity<int>
	{

		public string CropName { get; set; }

		public string? CropType { get; set; }

		public DateTime? plantingDate { get; set; }

		public string? LandPlantedType { get; set; }

		public string? AreaPlanted { get; set; }

		public string? CropHealthStatus { get; set; }

		public string PictureUrl { get; set; }

		public string? FarmerId { get; set; }
		[JsonIgnore] // Prevents infinite loop
		public Farmer Farmer { get; set; } //Navigation Property

		//retreive to not non nullable
		public int? LandId { get; set; }
		public Land Land { get; set; }

		public string? Description { get; set; }

		public ICollection<CropStage> Stages { get; set; } = new HashSet<CropStage>();


	}
}
