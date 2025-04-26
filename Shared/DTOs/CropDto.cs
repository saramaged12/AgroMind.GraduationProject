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
		public decimal TotalCost => Stages?.Sum(stage => stage.TotalCost) ?? 0;

		//TotalCost is a calculated property that dynamically computes the total cost of all stages in the Stages list.

	    //This ensures that the value of TotalCost is always up-to-date and reflects the current state of the Stages.


	}
}
