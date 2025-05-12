using AgroMind.GP.Core.Entities;
using Shared.DTOs;

namespace AgroMind.GP.Core.Specification
{
	public class CropSpecification : BaseSpecifications<Crop, int>
	{
		//For Get All Crops
		public CropSpecification() : base(c=>!c.IsDeleted)
		{
			//Includes.Add(c => c.Farmer);
			//Includes.Add(c => c.Land);
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");

		}

		//Get Crop By Id
		public CropSpecification(int id) : base(c => c.Id == id && !c.IsDeleted)
		{
			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");
		}

		// New constructor for recommended crops

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
			 (request.Budget >= c.TotalCost);

			AddInclude(c => c.Stages);
			StringIncludes.Add("Stages.Steps");

			AddOrderBy(c => c.TotalCost); // Sort by total cost ascending	

		}
	}
}
