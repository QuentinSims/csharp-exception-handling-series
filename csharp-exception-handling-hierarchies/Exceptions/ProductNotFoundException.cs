namespace csharp_exception_handling_hierarchies.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public int ProductId { get; }

        public ProductNotFoundException() : base() { }

        public ProductNotFoundException(string message) : base(message) { }

        public ProductNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public ProductNotFoundException(int productId)
            : base($"Product with ID {productId} was not found")
        {
            ProductId = productId;
        }
    }
}
