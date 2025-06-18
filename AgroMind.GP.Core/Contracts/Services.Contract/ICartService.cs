using Shared.DTOs;
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
		Task<CartDto> AddItemToCartAsync(string userId, AddToCartDto itemDto);
		Task<CartDto> UpdateItemQuantityAsync(string userId, int productId, int newQuantity);
		Task<CartDto> RemoveItemFromCartAsync(string userId, int productId);
		Task<bool> ClearUserCartAsync(string userId);
	}
}
