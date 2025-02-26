using AgroMind.GP.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Land :BaseEntity<int>
	{
		
		public string Name { get; set; }

		public string? Location { get; set; }

		public double? AreaSize { get; set; }
		public string? unitOfMeasurement { get; set; }

		public string? SoilType { get; set; }

		public string? waterSource { get; set; }

		public string? type { get; set; }

		public string? PictureUrl { get; set; }

		public ICollection<Crop> Crops { get; set; } = new HashSet<Crop>();
        
		public Farmer? Farmer { get; set; }

		public string? FarmerId { get; set; }

		public string status { get; set; }

		public string weatherCondition { get; set; }

		//public List<string> history { get; set; }

		public string currentCrop { get; set; }

		public bool isDeleted { get; set; }

		//areaSizeInM2 ----> mean????? and difference between areaSize and areaSizeInM2?????
		public double areaSizeInM2 { get; set; }
	}

}
