using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetCartWithItemsAsync(int cartId)
    {
        return await _dbSet
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        return await _dbSet
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task AddToCartAsync(int userId, int productId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await AddAsync(cart);
        }

        var existingItem = await GetCartItemAsync(cart.Id, productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _context.CartItems.Update(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            await _context.CartItems.AddAsync(cartItem);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItemQuantityAsync(int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int cartId)
    {
        var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> GetCartTotalAsync(int cartId)
    {
        return await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .SumAsync(ci => ci.Quantity * ci.Product.Price);
    }
}