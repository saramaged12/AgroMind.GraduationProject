using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class LandDTO
	{

		public int Id { get; set; }


		public string? LandName { get; set; }
		public double? Size { get; set; }


		public string? SoilType { get; set; }

		public string? waterSource { get; set; }

		public string? Location { get; set; }

		public string? PictureUrl { get; set; }



		public string? FarmerId { get; set; } //farmerId
		public string? IrrigationType { get; set; }
		
		
		
		
	}
}
