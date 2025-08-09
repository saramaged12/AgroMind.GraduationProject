using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class PaginatedResultDTO<TEntity>
	{
		public PaginatedResultDTO(int pageSize, int pageIndex, int totalCount, IReadOnlyList<TEntity> data)
		{
			PageSize = pageSize;
			PageIndex = pageIndex;
			TotalCount = totalCount;
			Data = data;
		}

		public int PageSize { get; set; }

		 public int PageIndex { get; set; }
		 public int TotalCount { get; set; }

		public IReadOnlyList<TEntity> Data { get; set; }
	}
}
