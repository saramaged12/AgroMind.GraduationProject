using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Step : BaseEntity<int>
	{
		public string? StepName { get; set; } 

		public string? Description { get; set; }
		public string? Tool { get; set; }
		public string? ToolImage { get; set; } 
		public int? DurationDays { get; set; } // Changed from DateTime? to int? (days)

		public string? Fertilizer { get; set; }
		//public int? FertilizerDuration { get; set; } 

		public decimal Cost { get; set; } // Cost of this step

		public int? StageId { get; set; }
		[JsonIgnore]
		public CropStage? Stage { get; set; }

		

	}

}
