using csharp_exception_handling_hierarchies.Data;
using csharp_exception_handling_hierarchies.Exceptions;
using csharp_exception_handling_hierarchies.Models;
using csharp_exception_handling_hierarchies.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace csharp_exception_handling_hierarchies.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(AppDbContext context, IProductService productService, IInventoryService inventoryService)
        {
            _context = context;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            // ArgumentException family for input validation
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.CustomerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.CustomerId),
                    "Customer ID must be greater than zero");

            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item", nameof(request.Items));

            // Validate customer exists and is active
            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null)
                throw new CustomerNotFoundException(request.CustomerId);

            if (!customer.IsActive)
                throw new InactiveCustomerException(customer.Id, customer.Name);

            var order = new Order
            {
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Items = new List<OrderItem>()
            };

            // Process each item
            foreach (var itemRequest in request.Items)
            {
                if (itemRequest.ProductId <= 0)
                    throw new ArgumentOutOfRangeException(
                        $"items[{request.Items.IndexOf(itemRequest)}].ProductId",
                        "Product ID must be greater than zero");

                if (itemRequest.Quantity <= 0)
                    throw new ArgumentOutOfRangeException(
                        $"items[{request.Items.IndexOf(itemRequest)}].Quantity",
                        "Quantity must be greater than zero");

                // Validate inventory (throws InsufficientInventoryException if not available)
                await _inventoryService.ValidateInventoryAsync(itemRequest.ProductId, itemRequest.Quantity);

                var product = await _productService.GetProductAsync(itemRequest.ProductId);

                order.Items.Add(new OrderItem
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderId),
                    "Order ID must be greater than zero");

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new InvalidOperationException($"Order with ID {orderId} not found");

            return order;
        }
    }
}
