using csharp_exception_handling_hierarchies.Exceptions;

namespace csharp_exception_handling_hierarchies.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductService _productService;

        public InventoryService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task ValidateInventoryAsync(int productId, int requestedQuantity)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId));

            if (requestedQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(requestedQuantity),
                    "Requested quantity must be greater than zero");

            var product = await _productService.GetProductAsync(productId);

            if (product.StockQuantity < requestedQuantity)
            {
                throw new InsufficientInventoryException(
                    product.Name, requestedQuantity, product.StockQuantity);
            }
        }

        public async Task ReserveInventoryAsync(int productId, int quantity)
        {
            await ValidateInventoryAsync(productId, quantity);

            // In a real system, this would update the database
            // For demo purposes, we'll just validate
        }
    }

}
