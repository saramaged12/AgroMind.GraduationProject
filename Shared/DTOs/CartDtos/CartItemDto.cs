using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CartDtos
{
    public class CartItemDto
    {
        public int Id { get; set; } //Product

        public required string ProductName { get; set; }

        public int Quantity { get; set; }


        public string? PictureUrl { get; set; }


        public decimal Price { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }
    }
}
