using csharp_exception_handling_hierarchies.Models;

namespace csharp_exception_handling_hierarchies.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int productId);
        Task<List<Product>> GetAllProductsAsync();
    }
}
