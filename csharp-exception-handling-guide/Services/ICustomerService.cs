using csharp_exception_handling_guide.Models;

namespace csharp_exception_handling_guide.Services
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerAsync(int customerId);
        Task<List<Customer>> GetAllCustomersAsync();
    }
}
