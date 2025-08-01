using csharp_exception_handling_guide.Data;
using csharp_exception_handling_guide.Models;
using Microsoft.EntityFrameworkCore;

namespace csharp_exception_handling_guide.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerAsync(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Customer ID must be greater than zero", nameof(customerId));
            return await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .ToListAsync();
        }
    }
}
