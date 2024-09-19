using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Infrastructure.Interface;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Service
{
    public class ProductService:IProduct
    {
        private readonly OrderDbContext _context;

        public ProductService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
