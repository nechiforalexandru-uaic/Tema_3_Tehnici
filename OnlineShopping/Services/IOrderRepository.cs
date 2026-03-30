using OnlineShopping.Models;

namespace OnlineShopping.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    Task<Order?> GetOrderWithItemsAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
}