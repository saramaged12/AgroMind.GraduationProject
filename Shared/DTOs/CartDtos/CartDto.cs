using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CartDtos
{
    public class CartDto
    {
        public required string Id { get; set; }  // will be the Redis Key ( Farmer.Id or a GUID for anonymous)
        public required IEnumerable<CartItemDto> Items { get; set; }

    }
}
