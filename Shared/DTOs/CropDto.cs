using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace AgroMind.GP.APIs.DTOs
{
	public class CropDto
	{
		public int Id { get; set; }
		public string? CropName { get; set; }

		public string? PictureUrl { get; set; } 
		public string? CropDescription { get; set; } 
		public List<CropStageDto> Stages { get; set; } = new List<CropStageDto>();

		// Null-safe calculated property
		public decimal TotalCost { get; set; } 
		public DateTime StartDate { get; set; }

		public DateTime LastStartDate { get; set; }

		public int Duration { get; set; }

	}
}
