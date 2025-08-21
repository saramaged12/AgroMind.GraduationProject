using AgroMind.GP.Core.Entities;

namespace AgroMind.GP.Core.Contracts.Repositories.Contract
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(string id);
        Task<Cart?> UpdateCartAsync(Cart cart,TimeSpan timeToLive);
        Task DeleteCartAsync(string Id);
    }
}
