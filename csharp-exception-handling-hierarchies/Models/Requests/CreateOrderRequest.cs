namespace csharp_exception_handling_hierarchies.Models.Requests
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
