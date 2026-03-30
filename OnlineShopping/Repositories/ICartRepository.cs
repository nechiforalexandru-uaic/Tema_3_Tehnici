using OnlineShopping.Models;

namespace OnlineShopping.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetCartWithItemsAsync(int cartId);
    Task<Cart?> GetCartByUserIdAsync(int userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId);
    Task AddToCartAsync(int userId, int productId, int quantity);
    Task UpdateCartItemQuantityAsync(int cartItemId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
    Task<decimal> GetCartTotalAsync(int cartId);
}