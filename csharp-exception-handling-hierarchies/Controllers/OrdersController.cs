using csharp_exception_handling_hierarchies.Exceptions;
using csharp_exception_handling_hierarchies.Models.Requests;
using csharp_exception_handling_hierarchies.Services;
using Microsoft.AspNetCore.Mvc;

namespace csharp_exception_handling_hierarchies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentNullException ex) // Null request (most specific first)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex) // Invalid ranges
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex) // Invalid input data 
            {
                return BadRequest(ex.Message);
            }
            catch (CustomerNotFoundException ex) // Domain-specific not found
            {
                return NotFound(ex.Message);
            }
            catch (ProductNotFoundException ex) // Domain-specific not found
            {
                return NotFound(ex.Message);
            }
            catch (InactiveCustomerException ex) // Business rule violations
            {
                return Conflict(ex.Message);
            }
            catch (InsufficientInventoryException ex) // Business rule violations
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex) // Other business rule violations
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex) // Database errors, unexpected failures (always last)
            {
                return StatusCode(500, "An error occurred processing your order");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _orderService.GetOrderAsync(id);
                return Ok(order);
            }
            catch (ArgumentOutOfRangeException ex) // Invalid input data
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) // Not found (translated from service)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex) // Database errors, unexpected failures
            {
                return StatusCode(500, "An error occurred retrieving the order");
            }
        }
    }
}
