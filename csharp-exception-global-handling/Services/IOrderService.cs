using csharp_exception_global_handling.DTOs;
using csharp_exception_global_handling.Models;

namespace csharp_exception_global_handling.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
    }
}
