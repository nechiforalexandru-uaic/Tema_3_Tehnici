using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(p => p.Category.ToLower() == category.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(searchTerm) ||
                        p.Description.Contains(searchTerm) ||
                        p.Category.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsInStockAsync()
    {
        return await _dbSet
            .Where(p => p.StockQuantity > 0)
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(int productId, int quantity)
    {
        var product = await _dbSet.FindAsync(productId);
        if (product == null)
            return false;

        if (product.StockQuantity + quantity < 0)
            return false;

        product.StockQuantity += quantity;
        await _context.SaveChangesAsync();
        return true;
    }
}