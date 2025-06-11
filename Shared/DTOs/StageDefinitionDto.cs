using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class StageDefinitionDto
	{
		public int Id { get; set; }
		public string? StageName { get; set; }
		public string? OptionalLink { get; set; }
		public List<StepDefinitionDto> Steps { get; set; } = new List<StepDefinitionDto>();

		public decimal? EstimatedCost { get; set; } 
													
		public int? CropId { get; set; }
	}
}
