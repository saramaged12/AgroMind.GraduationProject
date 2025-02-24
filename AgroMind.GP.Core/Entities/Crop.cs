using AgroMind.GP.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Crop : BaseEntity<int>
	{

		public string CropName { get; set; }

		public string CropType { get; set; }

		public DateTime plantingDate { get; set; }

		public string LandPlantedType { get; set; }

		public string AreaPlanted { get; set; }

		public string CropHealthStatus { get; set; }

		public string PictureUrl { get; set; }

		public string? FarmerId { get; set; }
		public Farmer Farmer { get; set; } //Navigation Property


		//public string LandId {  get; set; }

		public string CropDescription { get; set; }

	}
}
