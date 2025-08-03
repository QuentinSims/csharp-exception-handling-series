namespace csharp_exception_handling_hierarchies.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
