using csharp_exception_handling_hierarchies.Data;
using csharp_exception_handling_hierarchies.Exceptions;
using csharp_exception_handling_hierarchies.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace csharp_exception_handling_hierarchies.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            // ArgumentOutOfRangeException for invalid IDs
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId),
                    "Product ID must be greater than zero");

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                    throw new ProductNotFoundException(productId);

                return product;
            }
            catch (SqlException ex) when (ex.Number == -2) // Database timeout
            {
                throw new InvalidOperationException(
                    $"Database timeout while retrieving product {productId}. Please try again.", ex);
            }
            // Let other database exceptions bubble up
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
