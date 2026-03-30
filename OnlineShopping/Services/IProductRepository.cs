using OnlineShopping.Models;

namespace OnlineShopping.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetProductsInStockAsync();
    Task<bool> UpdateStockAsync(int productId, int quantity);
}