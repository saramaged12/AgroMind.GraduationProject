using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class RecommendRequestDTO
	{
		public DateTime FromDate {  get; set; }

		public DateTime ToDate { get; set; }

		public decimal Budget { get; set; }
	}
}
