using csharp_exception_global_handling.DTOs;
using csharp_exception_global_handling.Exceptions;
using csharp_exception_global_handling.Models;
using csharp_exception_global_handling.Repositories;

namespace csharp_exception_global_handling.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IUserService userService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Order ID must be positive", nameof(id));

            _logger.LogDebug("Getting order {OrderId}", id);

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} was not found");

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            // Verify user exists first
            await _userService.GetUserByIdAsync(userId); // Will throw if user doesn't exist

            _logger.LogDebug("Getting orders for user {UserId}", userId);
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            // Validate input
            if (request.Amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Amount), "Order amount must be positive");

            if (string.IsNullOrWhiteSpace(request.ProductName))
                throw new ArgumentException("Product name is required", nameof(request.ProductName));

            // Business rule: validate product codes
            if (request.ProductName.StartsWith("INVALID"))
                throw new InvalidProductCodeException(request.ProductName);

            // Get user and validate they exist
            var user = await _userService.GetUserByIdAsync(request.UserId);

            // Business rule: check if user has sufficient funds
            if (user.Balance < request.Amount)
                throw new InsufficientFundsException(request.UserId, user.Balance, request.Amount);

            _logger.LogInformation("Creating order for user {UserId}: {ProductName} - {Amount:C}",
                request.UserId, request.ProductName, request.Amount);

            var order = new Order
            {
                UserId = request.UserId,
                ProductName = request.ProductName,
                Amount = request.Amount
            };

            var createdOrder = await _orderRepository.CreateAsync(order);

            // Update user balance (simulate payment)
            await _userService.UpdateUserBalanceAsync(request.UserId,
                new UpdateUserBalanceRequest(user.Balance - request.Amount));

            return createdOrder;
        }
    }
}
