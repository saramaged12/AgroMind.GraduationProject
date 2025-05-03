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
		//For Get All Crops
		public StageSpecification() : base()
		{
			AddInclude(s => s.Steps);
			


		}

		//Get Crop By Id
		public StageSpecification(int id) : base(c => c.Id == id&&!c.IsDeleted)
		{
			AddInclude(s => s.Steps);
		}
	

	}
}
