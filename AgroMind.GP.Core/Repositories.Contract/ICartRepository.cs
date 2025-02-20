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
		Task<Cart?> GetCartAsync(string id);
		Task<Cart?> UpdateCartAsync(Cart cart);
		Task<bool> DeleteCartAsync(string Id);
		Task<Cart?> RemoveFromCart(string Id,int ItemId);
	}
}
