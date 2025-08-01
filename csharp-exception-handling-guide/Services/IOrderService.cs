using csharp_exception_handling_guide.Models.Requests;
using csharp_exception_handling_guide.Models;

namespace csharp_exception_handling_guide.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderAsync(int orderId);
        Task<Order> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request);
        Task<List<Order>> GetOrdersByCustomerAsync(int customerId);
    }
}
