using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.Core.Specification
{
	public class LandSpecification : BaseSpecifications<Land, int>
	{
		//For Get All Crops
		public LandSpecification() : base(l => !l.IsDeleted) //Filter Non Deleted
		{
			//if i want to include the related data ? Farmer Name and Crops Name
			Includes.Add(L => L.Farmer);
			//Includes.Add(L => L.Crops);

		}

		//Get Crop By Id
		public LandSpecification(int id) : base(l => l.Id == id && !l.IsDeleted)
		{
			AddInclude(l => l.Farmer); // Include Farmer for single land retrieval
		}

		// Constructor for Loading Land with Farmer for Authorization/Deletion
		public LandSpecification(int id, bool forAuthorization) : base(l => l.Id == id && !l.IsDeleted)
		{
			if (forAuthorization)
			{
				AddInclude(l => l.Farmer); 
			}
		}

		// Constructor for GetMyLands
		public LandSpecification(string farmerUserId, bool forMyLands) : base(l => !l.IsDeleted) // Filter non-deleted by default
		{
			if (forMyLands)
			{
				Criteria = l => l.FarmerId == farmerUserId; // Filter by the specific farmer's ID
				AddInclude(l => l.Farmer); // Include Farmer for display
										 
			}
		}
	}
}
