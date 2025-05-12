using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class RecommendRequest : BaseEntity<int>
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		[Column(TypeName = "decimal(18,2)")]

		public decimal Budget { get; set; }
	}

}
