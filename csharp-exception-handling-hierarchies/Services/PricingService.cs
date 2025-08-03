namespace csharp_exception_handling_hierarchies.Services
{
    public class PricingService : IPricingService
    {
        private readonly IProductService _productService;

        public PricingService(IProductService productService)
        {
            _productService = productService;
        }

        public decimal CalculateDiscountedPrice(string priceText, string discountPercentageText)
        {
            // ArgumentException for null/empty strings
            if (string.IsNullOrWhiteSpace(priceText))
                throw new ArgumentException("Price cannot be empty", nameof(priceText));

            if (string.IsNullOrWhiteSpace(discountPercentageText))
                throw new ArgumentException("Discount percentage cannot be empty", nameof(discountPercentageText));

            // FormatException for parsing failures
            if (!decimal.TryParse(priceText, out var price))
                throw new FormatException($"'{priceText}' is not a valid price format");

            if (!decimal.TryParse(discountPercentageText, out var discountPercentage))
                throw new FormatException($"'{discountPercentageText}' is not a valid percentage format");

            // ArgumentOutOfRangeException for invalid ranges
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(priceText), "Price cannot be negative");

            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercentageText),
                    "Discount percentage must be between 0 and 100");

            return price * (1 - discountPercentage / 100);
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            return product.Price;
        }
    }
}
