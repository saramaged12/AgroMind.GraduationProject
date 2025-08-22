using AgroMind.GP.Core.Entities;
using Shared.DTOs;
using Shared.DTOs.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Contracts.Services.Contract
{
    public interface ICartService
	{
		Task<CartDto> GetUserCartAsync(string userId);
		
		Task<CartDto> UpdateCartAsync(CartDto cart);
		
		Task ClearUserCartAsync(string Id);
	}
}
