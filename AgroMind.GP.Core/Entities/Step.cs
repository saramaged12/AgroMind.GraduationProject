using AgroMind.GP.Core.Entities.Identity;
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
		public int? DurationDays { get; set; } 

		public string? Fertilizer { get; set; }
		
		public int? StageId { get; set; }
		[JsonIgnore]
		public CropStage? Stage { get; set; }

		//Estimated And Actual Costs , Dates
		public decimal? EstimatedCost { get; set; } // Estimated Cost 


		public decimal? ActualCost { get; set; }
		public DateTime? ActualStartDate { get; set; }
		
		public DateTime? PlannedStartDate { get; set; }


	}

}
