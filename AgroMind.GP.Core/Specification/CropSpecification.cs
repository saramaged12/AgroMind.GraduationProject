using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.Core.Specification
{
	public class CropSpecification : BaseSpecifications<Crop, int>
	{
		//For Get All Crops
		public CropSpecification() : base()
		{
			//Includes.Add(c => c.Farmer);
			//Includes.Add(c => c.Land);
			AddInclude(c => c.Stages);


		}

		//Get Crop By Id
		public CropSpecification(int id) : base(c => c.Id == id && !c.IsDeleted)
		{
			AddInclude(c => c.Stages);
		}
	}
}
