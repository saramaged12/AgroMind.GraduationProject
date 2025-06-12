using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class CropDefinitionDto
	{
		public int Id { get; set; } 
		public string? CropName { get; set; }
		public string? PictureUrl { get; set; }
		public string? CropDescription { get; set; }
		public List<StageDefinitionDto> Stages { get; set; } = new List<StageDefinitionDto>();

		public DateTime StartDate { get; set; }
		public DateTime LastStartDate { get; set; }
		public int Duration { get; set; }

		public CropPlanType? PlanType { get; set; } 
		public int? LandId { get; set; } 
	}
}
