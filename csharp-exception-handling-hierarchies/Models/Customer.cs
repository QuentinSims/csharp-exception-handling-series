namespace csharp_exception_handling_hierarchies.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
