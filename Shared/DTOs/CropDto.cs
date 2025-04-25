namespace AgroMind.GP.APIs.DTOs
{
	public class CropDto
	{
		public int Id { get; set; }
		public string? CropName { get; set; }

		public string? PictureUrl { get; set; } 
		public string? CropDescription { get; set; } 
		public List<CropStageDto> Stages { get; set; } = new List<CropStageDto>(); 



	}
}
