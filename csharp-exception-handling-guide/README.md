# Part 1: Exception Fundamentals

This project demonstrates the basic concepts from the blog post "C# Exception Fundamentals – Part 1: The Mechanics of Exception Handling".

## What This Demonstrates

- **Simple architecture**: Database ➡️ Service ➡️ Controller
- **Exception bubbling**: How exceptions flow up from Entity Framework through your service to the controller
- **Proper exception handling**: When to catch, when to let bubble up
- **Stack trace preservation**: Using `throw` vs `throw ex`

## How to Run

1. Clone this folder
2. Run `dotnet run` in the terminal or press F5 in Visual Studio
3. Navigate to `https://localhost:7045/swagger` to test the API

## Key Files to Examine

- **Controllers/OrdersController.cs** - Shows proper exception handling at the controller level
- **Services/OrderService.cs** - Demonstrates letting database exceptions bubble up naturally
- **Test endpoint** - Use `/api/orders/test-exceptions/{scenario}` to see different exception types in action

## Test Scenarios

Try these endpoints to see exception handling in action:

- `POST /api/orders` with invalid data (empty customer name, negative amount)
- `POST /api/orders` with amount > $10,000 (business rule violation)
- `POST /api/orders/test-exceptions/argument` - See ArgumentException handling
- `POST /api/orders/test-exceptions/invalidop` - See InvalidOperationException handling
- `POST /api/orders/test-exceptions/sql` - See generic Exception handling

## Key Concepts Demonstrated

1. **Service layer doesn't catch database exceptions** - they bubble up naturally
2. **Controller handles exceptions appropriately** - translating to HTTP status codes
3. **Different exception types map to different HTTP responses**
4. **Logging at the controller level** - where you decide how to respond to the outside world