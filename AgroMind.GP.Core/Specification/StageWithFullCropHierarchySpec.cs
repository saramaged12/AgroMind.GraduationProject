using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class StageWithFullCropHierarchySpec : BaseSpecifications<CropStage, int>
	{
		public StageWithFullCropHierarchySpec(int stageId) : base(s => s.Id == stageId && !s.IsDeleted)
		{
			AddInclude(s => s.Steps);
			AddInclude(s => s.Crop);
			StringIncludes.Add("Crop.Stages");
			StringIncludes.Add("Crop.Stages.Steps");
			AddInclude(s => s.Crop.Land);
		}


	}
}
