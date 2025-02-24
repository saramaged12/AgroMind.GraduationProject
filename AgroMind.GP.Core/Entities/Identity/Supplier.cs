using AgroMind.GP.Core.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Identity
{
	public class Supplier : AppUser
	{
		public int inventoryCount { get; set; }
		public ICollection<Product>? Products { get; set; } = new HashSet<Product>(); //M
	}
}
