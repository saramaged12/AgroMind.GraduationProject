using AgroMind.GP.Core.Entities;
using Shared.DTOs;

namespace AgroMind.GP.Core.Specification
{
	public class CropSpecification : BaseSpecifications<Crop, int>
	{
		// 1. Constructor for Get All Crops 
		public CropSpecification() : base(c=>!c.IsDeleted)
		{
			//Includes.Add(c => c.Farmer);
			//Includes.Add(c => c.Land);
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");

		}

		// 2. Constructor for Get Crop By Id (specific ID)
		public CropSpecification(int id) : base(c => c.Id == id && !c.IsDeleted)
		{
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");
		}

		// 3. Constructor for recommended crops (GetRecommendedCropsAsync)
		public CropSpecification(RecommendRequestDTO request) : base()
		{
			Criteria = c =>
			 !c.IsDeleted &&
			 // Find the earliest and latest possible planting dates
			 (
				 // The earliest date can plant at
				 (c.StartDate <= c.LastStartDate) &&
				 (request.FromDate <= request.ToDate) &&
				 // Calculate possible planting dates
				 (
					 // Earliest planting date
					 (System.DateTime.Compare(c.StartDate, request.FromDate) > 0 ? c.StartDate : request.FromDate)
					 <=
					 // Latest planting date 
					 (System.DateTime.Compare(c.LastStartDate, request.ToDate.AddDays(-c.Duration)) < 0 ? c.LastStartDate : request.ToDate.AddDays(-c.Duration))
				 )
			 )
			 &&
			 (request.Budget >= c.TotalEstimatedCost);

			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");

			AddOrderBy(c => c.TotalEstimatedCost); // Sort by total cost ascending	

		}


		// 4. Constructor for getting a SINGLE PlanInfo (GetPlanAndCreatorInfoAsync)
		// Includes: Creator, Land, Stages, Steps
		public CropSpecification(int id, bool includeCreatorInfo) : base(c => c.Id == id && !c.IsDeleted)
		{
			if (includeCreatorInfo)
			{
				AddInclude(c => c.Creator); // Include the AppUser who created the crop
				AddInclude(c => c.Land);    // Include Land to determine if it's a FarmerPlan and get FarmerId
			}
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");
		}


		// 5. Constructor for getting ALL Plans with Creator/Land Info (GetAllPlansWithCreatorInfoAsync)
		// Includes: Creator, Land, Stages, Steps
		public CropSpecification(bool includeCreatorAndLandForAll) : base(c => !c.IsDeleted) // Filters non-deleted
		{
			if (includeCreatorAndLandForAll) // Flag to ensure this specific constructor is used
			{
				AddInclude(c => c.Creator); // Include the AppUser who created the crop
				AddInclude(c => c.Land);    // Include Land to determine if it's a FarmerPlan
				AddInclude(c => c.Stages);
			    StringIncludes.Add("Stages.Steps");
				// Optional: Add default ordering if beneficial for this view (e.g., by CreationDate)
				// AddOrderByDescending(c => c.CreatedAt);
			}
		}

		//// Specification for GetMyPlansAsync
		//public CropSpecification(string farmerUserId, bool forMyPlans) : base(c => !c.IsDeleted)
		//{
		//	if (forMyPlans) // This boolean flag distinguishes it from other constructors
		//	{
		//		Criteria = c => c.PlanType == CropPlanType.FarmerPlan && c.Land != null && c.Land.FarmerId == farmerUserId && !c.IsDeleted;
		//		AddInclude(c => c.Land); // Include Land to confirm FarmerId
		//		AddInclude(c => c.Stages);
		//		StringIncludes.Add("Stages.Steps");

		//	}
		//	// If forMyPlans is false, it behaves like default constructor or can throw error
		//}


		// Specification for GetMyPlansAsync
		public CropSpecification(string farmerUserId, bool forMyPlans)
			// The entire filtering logic is in the base constructor call.
			: base(crop =>
				!crop.IsDeleted &&
				crop.PlanType == CropPlanType.FarmerPlan &&
		    	crop.Land != null &&
				crop.Land.FarmerId == farmerUserId

			)
		{
			
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");
			AddInclude(c => c.Land);
		}

		// 7. Constructor for Loading Full Crop Graph for Update/Recalculation (used by UpdateCrops, AdoptRecommendedCropAsync, UpdateActualsForCropAsync)
		// Includes: Land (for auth), Stages, Steps
		public CropSpecification( bool forUpdate,int cropId) : base(c => c.Id == cropId && !c.IsDeleted)
		{
			if (forUpdate)
			{
				AddInclude(c => c.Stages);
				StringIncludes.Add("Stages.Steps");
				AddInclude(c => c.Land); // For authorization check (especially in UpdateActuals)
				AddInclude(c => c.Creator); // Include Creator for AdoptRecommendedCrop audit source
			}
		}
	}
}

