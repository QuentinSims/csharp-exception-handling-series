namespace csharp_exception_handling_hierarchies.Exceptions
{
    public class InsufficientInventoryException : Exception
    {
        public string ProductName { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        // Always include all three standard constructors
        public InsufficientInventoryException() : base() { }

        public InsufficientInventoryException(string message) : base(message) { }

        public InsufficientInventoryException(string message, Exception innerException)
            : base(message, innerException) { }

        // Business-specific constructor
        public InsufficientInventoryException(string productName, int requested, int available)
            : base($"Insufficient inventory for {productName}. Requested: {requested}, Available: {available}")
        {
            ProductName = productName;
            RequestedQuantity = requested;
            AvailableQuantity = available;
        }
    }
}
