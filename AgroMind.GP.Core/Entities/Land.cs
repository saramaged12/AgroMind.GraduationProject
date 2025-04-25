using AgroMind.GP.Core.Entities.Identity;


namespace AgroMind.GP.Core.Entities
{
	public class Land : BaseEntity<int>
	{

		public string? LandName { get; set; }
		public double? Size { get; set;}
		
		public string? SoilType { get; set; }

		public string? waterSource { get; set; }

		public string? PictureUrl { get; set; }

		public ICollection<Crop> Crops { get; set; } = new HashSet<Crop>();

		public Farmer? Farmer { get; set; }

		public string? FarmerId { get; set; }

		public string? IrrigationType { get; set; }
		public DateTime StartDate { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

	}

}
