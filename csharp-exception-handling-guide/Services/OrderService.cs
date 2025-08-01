using csharp_exception_handling_guide.Data;
using csharp_exception_handling_guide.Models.Requests;
using csharp_exception_handling_guide.Models;
using Microsoft.EntityFrameworkCore;

namespace csharp_exception_handling_guide.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly ICustomerService _customerService;

        public OrderService(AppDbContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request.CustomerId <= 0)
                throw new ArgumentException("Customer ID must be greater than zero", nameof(request.CustomerId));

            if (string.IsNullOrWhiteSpace(request.ProductName))
                throw new ArgumentException("Product name cannot be empty", nameof(request.ProductName));

            if (request.Amount <= 0)
                throw new ArgumentException("Order amount must be greater than zero", nameof(request.Amount));

            var customer = await _customerService.GetCustomerAsync(request.CustomerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer with ID {request.CustomerId} does not exist");

            var pendingOrdersCount = await _context.Orders
                .CountAsync(o => o.CustomerId == request.CustomerId && o.Status == OrderStatus.Pending);

            if (pendingOrdersCount >= 5)
                throw new InvalidOperationException($"Customer {request.CustomerId} already has the maximum number of pending orders");

            var order = new Order
            {
                CustomerId = request.CustomerId,
                ProductName = request.ProductName,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be greater than zero", nameof(orderId));

            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request)
        {
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be greater than zero", nameof(orderId));

            var order = await GetOrderAsync(orderId);
            if (order == null)
                throw new InvalidOperationException($"Order with ID {orderId} not found");

            if (order.Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot change status of delivered orders");

            if (request.Status != OrderStatus.Cancelled && !IsValidStatusTransition(order.Status, request.Status))
                throw new InvalidOperationException($"Cannot change order status from {order.Status} to {request.Status}");

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID must be greater than zero", nameof(customerId));

            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
        {
            return current switch
            {
                OrderStatus.Pending => next == OrderStatus.Processing,
                OrderStatus.Processing => next == OrderStatus.Shipped,
                OrderStatus.Shipped => next == OrderStatus.Delivered,
                _ => false
            };
        }
    }

}
