using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class StageSpecification : BaseSpecifications<CropStage, int>
	{
		//For Get All Stagess
		public StageSpecification() : base()
		{
			AddInclude(s => s.Steps);



		}

		//Get Stage By Id
		public StageSpecification(int id) : base(c => c.Id == id && !c.IsDeleted)
		{
			AddInclude(s => s.Steps);
		}

		// 3. Constructor for Loading Stage Graph for Update/Recalculation
		// Used by StageService.UpdateStage, StepService.AddStepAsync, StepService.UpdateStep
		// Includes: Steps, and parent Crop (for PlanType check)
		public StageSpecification(int stageId, bool forUpdate) : base(s => s.Id == stageId && !s.IsDeleted)
		{
			if (forUpdate)
			{
				AddInclude(s => s.Steps); // Include steps for stage recalculation
				AddInclude(s => s.Crop);  // Include parent crop to check PlanType for actuals calculations
			}
		}

		//// 4. Constructor for Loading Stage with Full Crop Hierarchy(for RecordStepCompletionAsync in CropService)
		//// Includes: Steps, Crop, Crop.Stages, Crop.Stages.Steps, Crop.Land
		//public StageSpecification StageWithFullCropHierarchySpec(int stageId)
		//{
		//	return new StageSpecification.StageWithFullCropHierarchySpec(stageId);
		//}


	}
}