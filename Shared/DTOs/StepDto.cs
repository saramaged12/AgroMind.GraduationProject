namespace AgroMind.GP.APIs.DTOs
{
	public class StepDto
	{
		public int Id { get; set; } 

		public string? StepName { get; set; } 

		public string? Description { get; set; }
		public string? Tool { get; set; }
		public string? ToolImage { get; set; } 

		public int? DurationDays { get; set; }

		public string? Fertilizer { get; set; }

		public decimal? EstimatedCost { get; set; } // Estimated Cost 
		

		public decimal? ActualCost { get; set; } // FOR DISPLAY / FOR SENDING BACK UPDATED ACTUALS
		public DateTime? ActualStartDate { get; set; } 
		public DateTime? PlannedStartDate { get; set; } 
		
		public string? CreatorId { get; set; }
		
		public int? StageId { get; set; } 
	}
}
