using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Interface
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<bool> AddOrderAsync(Order order);
    }
}
