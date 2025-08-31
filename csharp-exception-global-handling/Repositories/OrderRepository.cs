using csharp_exception_global_handling.Data;
using csharp_exception_global_handling.Models;
using Microsoft.EntityFrameworkCore;

namespace csharp_exception_global_handling.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            _logger.LogDebug("Fetching order with ID: {OrderId}", id);
            return await _context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            _logger.LogDebug("Fetching orders for user: {UserId}", userId);
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _logger.LogDebug("Creating new order for user: {UserId}", order.UserId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
