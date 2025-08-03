# Exception Hierarchies - Part 2 Demo API

This project demonstrates the exception hierarchy concepts covered in the blog post "C# Exception Fundamentals – Part 2: Choosing the Right Exception for the Job".

## What This Demonstrates

### Built-in Exception Types
- **ArgumentException family**: Input validation with specific exception types
- **InvalidOperationException**: Business rule violations and state-related errors
- **FormatException**: Data parsing and format validation
- **Exception ordering**: Most specific to least specific in catch blocks

### Custom Exception Design
- **Proper inheritance**: All custom exceptions inherit from Exception
- **Standard constructors**: All three required constructors implemented
- **Business context**: Custom properties that carry domain-specific information
- **Clear messages**: Automatically generated, consistent error messages

### Exception Translation
- **Service layer**: Translating low-level exceptions into business-meaningful ones
- **Preserving context**: Using inner exceptions to maintain the full error chain
- **Appropriate scoping**: Only translating when it adds business value

## Project Structure

```
ExceptionHierarchies.Api/
├── Controllers/          # Exception handling and HTTP status mapping
├── Services/            # Business logic with proper exception usage
├── Exceptions/          # Custom domain-specific exceptions
├── Models/              # Data models and request objects
├── Data/                # Entity Framework DbContext
└── Program.cs           # Application setup
```

## Exception Scenarios to Test

### ArgumentException Family (400 Bad Request)
1. **ArgumentNullException**: POST `/api/orders` with null body
2. **ArgumentOutOfRangeException**: 
   - GET `/api/products/-1` (negative ID)
   - POST `/api/orders` with CustomerId = 0
3. **ArgumentException**: POST `/api/orders` with empty items array

### FormatException (400 Bad Request)
- POST `/api/pricing/calculate-discount` with:
  ```json
  {
    "priceText": "abc",
    "discountPercentageText": "xyz"
  }
  ```

### Custom Domain Exceptions
1. **CustomerNotFoundException** (404 Not Found):
   - POST `/api/orders` with CustomerId = 999
2. **ProductNotFoundException** (404 Not Found):
   - GET `/api/products/999`
3. **InactiveCustomerException** (409 Conflict):
   - POST `/api/orders` with CustomerId = 2 (inactive customer)
4. **InsufficientInventoryException** (409 Conflict):
   - POST `/api/orders` requesting 10 units of product ID 2 (out of stock)

### Exception Translation Examples
- Database timeouts get translated to InvalidOperationException
- Not found entities become domain-specific NotFoundException types
- SQL exceptions bubble up unless specifically handled

## How to Run

1. Clone the repository
2. Navigate to the project folder
3. Run `dotnet restore`
4. Run `dotnet run`
5. Open your browser to `https://localhost:7xxx/swagger`

## Test Data

- **Products**: Gaming Laptop (5 in stock), Wireless Mouse (0 in stock), Mechanical Keyboard (15 in stock)
- **Customers**: John Doe (active), Jane Smith (inactive)

## Key Learning Points

1. **Exception Specificity**: Notice how different input validation scenarios throw different ArgumentException types
2. **Custom Exception Structure**: All custom exceptions follow the same pattern with standard constructors
3. **Catch Block Ordering**: Controllers catch from most specific to most general exceptions
4. **Business Context**: Custom exceptions carry relevant properties for better error handling
5. **Exception Translation**: Services translate infrastructure exceptions into business-meaningful ones
6. **Preservation of Context**: Inner exceptions maintain the full error chain for debugging

## Stack Trace Examples

Try these scenarios to see different stack traces:
- Trigger a custom exception to see clean business error traces
- Cause a database timeout simulation to see exception translation
- Send invalid input to see ArgumentException variations
