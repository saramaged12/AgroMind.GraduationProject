using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class StepDefinitionDto
	{


		//No Actuals or TotalCost in Definition DTOs
		public int Id { get; set; }
		public string? StepName { get; set; }
		public string? Description { get; set; }
		public string? Tool { get; set; }
		public string? ToolImage { get; set; }
		public int? DurationDays { get; set; }
		public string? Fertilizer { get; set; }

		public decimal? EstimatedCost { get; set; }
		public DateTime? PlannedStartDate { get; set; }

		public int? StageId { get; set; }

	}
}
