using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Step : BaseEntity<int>
	{
		public string? StepName { get; set; }

		public int? cost { get; set; }

		public string? tool { get; set; }

		public string? description { get; set; }

		public DateTime? Duration { get; set; }

		public CropStage? Stage { get; set; }

		public int? StageId { get; set; }


	}

}
