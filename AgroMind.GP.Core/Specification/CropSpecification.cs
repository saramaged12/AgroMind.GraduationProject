using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class CropSpecification :BaseSpecifications<Crop,int>
	{
		//For Get All Crops
		public CropSpecification() : base()
		{
			Includes.Add(c => c.Farmer);
			
		}

		//Get Crop By Id
		public CropSpecification(int id) : base(c => c.Id == id)
		{
		}
	}
}
