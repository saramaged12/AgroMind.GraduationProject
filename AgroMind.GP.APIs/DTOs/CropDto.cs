namespace AgroMind.GP.APIs.DTOs
{
	public class CropDto
	{
		public int Id { get; set; }
		public string CropName { get; set; }
		public string? Description { get; set; }
		public string? PictureUrl { get; set; }  // Matches frontend's `cropImage`
		public List<CropStageDto> Stages { get; set; } = new List<CropStageDto>();  // List of stages
		public decimal TotalCost { get; set; } // Calculated sum of stage costs



		//public string CropType { get; set; }

		//public DateTime plantingDate { get; set; }

		//public string LandPlantedType { get; set; }

		//public string AreaPlanted { get; set; }

		//public string CropHealthStatus { get; set; }

		//public string PictureUrl { get; set; }

		//public string? FarmerId { get; set; }


	}
}
