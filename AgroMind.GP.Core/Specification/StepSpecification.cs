using AgroMind.GP.Core.Contracts.Services.Contract;
using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class StepSpecification :BaseSpecifications<Step,int>
	{
		//Get AllSteps
		public StepSpecification() : base(s => !s.IsDeleted)
		{

		}
		//2. Get  Step By Id
		public StepSpecification(int id):base(s=>s.Id==id && !s.IsDeleted)
		{
			AddInclude(s => s.Stage); // Include parent stage
		}


		// 3. Constructor for Loading Full Step Graph for Update/Recalculation(for RecordStepCompletionAsync, StepService.AddStep, StepService.UpdateStep)
		// Includes: Stage, Stage.Steps, Stage.Crop, Stage.Crop.Stages, Stage.Crop.Stages.Steps, Stage.Crop.Land
		public StepSpecification(int stepId, bool forUpdate) : base(s => s.Id == stepId && !s.IsDeleted)
		{
			if (forUpdate)
			{
				AddInclude(s => s.Stage);
				StringIncludes.Add("Stage.Steps");
				AddInclude(s => s.Stage.Crop);
				StringIncludes.Add("Stage.Crop.Stages");
				StringIncludes.Add("Stage.Crop.Stages.Steps");
				AddInclude(s => s.Stage.Crop.Land); // For authorization check
			}
		}
	}
}
