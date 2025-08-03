namespace csharp_exception_handling_hierarchies.Services
{
    public interface IInventoryService
    {
        Task ValidateInventoryAsync(int productId, int requestedQuantity);
        Task ReserveInventoryAsync(int productId, int quantity);
    }
}
