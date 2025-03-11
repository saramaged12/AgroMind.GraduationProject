namespace AgroMind.GP.APIs.DTOs
{
	public class CropDto
	{
		public int Id { get; set; }
		public string CropName { get; set; }
		public string Description { get; set; }

		public string ImageUrl { get; set; } // Store image path or URL

		public string StageName { get; set; }

		public string? FarmerName { get; set; }



		//public string CropType { get; set; }

		//public DateTime plantingDate { get; set; }

		//public string LandPlantedType { get; set; }

		//public string AreaPlanted { get; set; }

		//public string CropHealthStatus { get; set; }

		//public string PictureUrl { get; set; }

		//public string? FarmerId { get; set; }


	}
}
