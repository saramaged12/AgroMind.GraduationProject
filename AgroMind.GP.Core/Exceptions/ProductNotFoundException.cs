using AgroMind.GP.Core.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Exceptions
{
	public sealed class ProductNotFoundException(int id) : NotFoundException(nameof(Product), id)
	{
		
	}
}
