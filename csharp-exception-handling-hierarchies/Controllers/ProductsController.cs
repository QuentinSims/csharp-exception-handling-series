using csharp_exception_handling_hierarchies.Exceptions;
using csharp_exception_handling_hierarchies.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_exception_handling_hierarchies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex) // Database errors, unexpected failures
            {
                return StatusCode(500, "An error occurred retrieving products");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                return Ok(product);
            }
            catch (ArgumentOutOfRangeException ex) // Invalid input data
            {
                return BadRequest(ex.Message);
            }
            catch (ProductNotFoundException ex) // Domain-specific not found
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex) // Business rule violations (like timeouts)
            {
                return StatusCode(503, ex.Message); // Service Unavailable for timeouts
            }
            catch (Exception ex) // Database errors, unexpected failures
            {
                return StatusCode(500, "An error occurred retrieving the product");
            }
        }
    }
}
