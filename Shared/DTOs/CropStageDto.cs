namespace AgroMind.GP.APIs.DTOs
{
	public class CropStageDto
	{
		public int Id { get; set; }

		public string? StageName { get; set; }

		public string? OptionalLink { get; set; }

		public List<StepDto> Steps { get; set; } = new List<StepDto>();

		
		public decimal? EstimatedCost { get; set; }

		public decimal? TotalEstimatedCost { get; set; }
		public decimal? ActualCost { get; set; } // FOR DISPLAY / FOR SENDING BACK UPDATED ACTUALS

	
		public decimal? TotalActualCost { get; set; } 

		public int? CropId { get; set; } 
		
		public string? CreatorId { get; set; }
	
	}
}
