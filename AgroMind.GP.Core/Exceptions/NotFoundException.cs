using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Exceptions
{
	public  class NotFoundException : SystemException
	{
		public  NotFoundException(string name, object key) //entity name , id of entity
			: base($"The {name} with id : ({key}) is not found.")
		
		{
		}
	}
}
