namespace AgroMind.GP.APIs.DTOs
{
	public class CropStageDto
	{
		public int Id { get; set; }

		public string? StageName { get; set; }

		public string? OptionalLink { get; set; }

		public List<StepDto> Steps { get; set; } = new List<StepDto>();

		public decimal Cost { get; set; } // Cost of the stage it self
		public decimal TotalCost { get; set; }

	}
}
