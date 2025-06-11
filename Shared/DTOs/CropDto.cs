//using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.APIs.DTOs
{
	public class CropDto
	{
		public int Id { get; set; }
		public string? CropName { get; set; }

		public string? PictureUrl { get; set; } 
		public string? CropDescription { get; set; } 
		public List<CropStageDto> Stages { get; set; } = new List<CropStageDto>();

		public decimal? TotalEstimatedCost { get; set; }
		public decimal? TotalActualCost { get; set; } // FOR DISPLAY / FOR SENDING BACK UPDATED ACTUALS

		public DateTime StartDate { get; set; }

		public DateTime LastStartDate { get; set; }

		public int Duration { get; set; }

		public string? CreatorId { get; set; }

		public CropPlanType? PlanType { get; set; }

		public int? LandId { get; set; } 
	}
}
