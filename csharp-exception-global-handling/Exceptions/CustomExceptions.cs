namespace csharp_exception_global_handling.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public int UserId { get; }

        public UserNotFoundException() : base() { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public UserNotFoundException(int userId)
            : base($"User with ID {userId} was not found")
        {
            UserId = userId;
        }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base() { }

        public InsufficientFundsException(string message) : base(message) { }

        public InsufficientFundsException(string message, Exception innerException)
            : base(message, innerException) { }

        public int UserId { get; }
        public decimal CurrentBalance { get; }
        public decimal RequestedAmount { get; }

        public InsufficientFundsException(int userId, decimal currentBalance, decimal requestedAmount)
            : base($"User {userId} has insufficient funds. Balance: {currentBalance:C}, Requested: {requestedAmount:C}")
        {
            UserId = userId;
            CurrentBalance = currentBalance;
            RequestedAmount = requestedAmount;
        }
    }

    public class InvalidProductCodeException : Exception
    {
        public string ProductCode { get; }

        public InvalidProductCodeException() : base() { }

        public InvalidProductCodeException(string productCode)
            : base($"Product code '{productCode}' is not valid")
        {
            ProductCode = productCode;
        }
    }

    public class DuplicateEmailException : Exception
    {
        public string Email { get; }
        public DuplicateEmailException() : base() { }

        public DuplicateEmailException(string message, Exception innerException)
            : base(message, innerException) { }

        public DuplicateEmailException(string email)
            : base($"A user with email '{email}' already exists")
        {
            Email = email;
        }
    }

}
