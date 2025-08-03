using csharp_exception_handling_hierarchies.Models;
using csharp_exception_handling_hierarchies.Models.Requests;

namespace csharp_exception_handling_hierarchies.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order> GetOrderAsync(int orderId);
    }
}
