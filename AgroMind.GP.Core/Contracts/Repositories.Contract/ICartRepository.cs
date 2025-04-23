using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.Core.Contracts.Repositories.Contract
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(string Farmerid);
        Task<Cart?> UpdateCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(string FarmerId);
        Task<Cart?> RemoveFromCart(string FarmerId, int ItemId);
    }
}
