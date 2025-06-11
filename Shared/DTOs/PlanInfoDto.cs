using AgroMind.GP.APIs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class PlanInfoDto
	{
		public CropDto Crop { get; set; }
		public string CreatorEmail { get; set; }
		public string CreatorRole { get; set; }
		public string PlanType { get; set; }
	}

}

