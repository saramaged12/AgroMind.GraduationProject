﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.ProductModule
{
	public class Category :BaseEntity<int>
	{
		//public int Id { get; set; }

		public string CategoryName { get; set; }

		//public string imageUrl { get; set; }

		//public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
