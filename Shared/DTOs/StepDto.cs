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
		public decimal Cost { get; set; } 


	}
}
