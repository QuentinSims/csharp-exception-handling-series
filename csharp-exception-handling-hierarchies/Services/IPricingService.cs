namespace csharp_exception_handling_hierarchies.Services
{
    public interface IPricingService
    {
        decimal CalculateDiscountedPrice(string priceText, string discountPercentageText);
        Task<decimal> GetProductPriceAsync(int productId);
    }
}
