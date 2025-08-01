namespace csharp_exception_handling_guide.Models.Requests
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
