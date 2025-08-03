namespace csharp_exception_handling_hierarchies.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public int CustomerId { get; }

        // Always include all three standard constructors
        public CustomerNotFoundException() : base() { }

        public CustomerNotFoundException(string message) : base(message) { }

        public CustomerNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        // Business-specific constructor
        public CustomerNotFoundException(int customerId)
            : base($"Customer with ID {customerId} was not found")
        {
            CustomerId = customerId;
        }
    }
}
