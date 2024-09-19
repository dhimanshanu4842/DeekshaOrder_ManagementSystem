using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Interface
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
