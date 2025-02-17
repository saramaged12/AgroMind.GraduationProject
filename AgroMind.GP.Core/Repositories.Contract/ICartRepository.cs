using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Repositories.Contract
{
	public interface ICartRepository
	{
		Task<Cart?> GetCartAsync(string CartId);
		Task<Cart?> UpdateCarttAsync(Cart cart);
		Task<bool> DeleteCartAsync(string CartId);
		Task<Cart?> RemoveFromCart(string CartId,int ItemId);
	}
}
