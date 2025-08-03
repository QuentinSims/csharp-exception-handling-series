namespace csharp_exception_handling_hierarchies.Exceptions
{
    public class InactiveCustomerException : Exception
    {
        public int CustomerId { get; }
        public string CustomerName { get; }

        public InactiveCustomerException() : base() { }

        public InactiveCustomerException(string message) : base(message) { }

        public InactiveCustomerException(string message, Exception innerException)
            : base(message, innerException) { }

        public InactiveCustomerException(int customerId, string customerName)
            : base($"Customer '{customerName}' (ID: {customerId}) is inactive and cannot place orders")
        {
            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}
