﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Identity
{
	public class Farmer : AppUser
	{
		//Navigation Properties are used to define relationships between entities in EF Core

		public ICollection<Crop>? Crops { get; set; } = new HashSet<Crop>();
		
		//public ICollection<Land>? Lands { get; set; } = new HashSet<Land>();
		//public ICollection<Order>? Orders { get; set; } = new HashSet<Order>();
	}
}
