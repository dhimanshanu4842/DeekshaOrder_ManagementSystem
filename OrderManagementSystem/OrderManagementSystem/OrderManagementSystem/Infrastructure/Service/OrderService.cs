using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Infrastructure.Interface;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Service
{
    public class OrderService: IOrder
    {

        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
        public async Task<bool> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync() > 0;
        }
    }}
