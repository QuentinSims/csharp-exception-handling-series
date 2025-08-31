namespace csharp_exception_global_handling.DTOs
{
    public record CreateUserRequest(string Name, string Email, decimal InitialBalance = 0);

    public record CreateOrderRequest(int UserId, string ProductName, decimal Amount);

    public record UpdateUserBalanceRequest(decimal NewBalance);
}
