using csharp_exception_handling_hierarchies.Exceptions;
using csharp_exception_handling_hierarchies.Models.Requests;
using csharp_exception_handling_hierarchies.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_exception_handling_hierarchies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpPost("calculate-discount")]
        public IActionResult CalculateDiscountedPrice([FromBody] PriceCalculationRequest request)
        {
            try
            {
                var discountedPrice = _pricingService.CalculateDiscountedPrice(
                    request.PriceText, request.DiscountPercentageText);
                return Ok(new { OriginalPrice = request.PriceText, DiscountedPrice = discountedPrice });
            }
            catch (FormatException ex) // Invalid format for numbers
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex) // Values outside valid ranges
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex) // Empty/null input
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) // Unexpected failures
            {
                return StatusCode(500, "An error occurred calculating the price");
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductPrice(int productId)
        {
            try
            {
                var price = await _pricingService.GetProductPriceAsync(productId);
                return Ok(new { ProductId = productId, Price = price });
            }
            catch (ArgumentOutOfRangeException ex) // Invalid input data
            {
                return BadRequest(ex.Message);
            }
            catch (ProductNotFoundException ex) // Domain-specific not found
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex) // Database errors, unexpected failures
            {
                return StatusCode(500, "An error occurred retrieving the product price");
            }
        }
    }

}
