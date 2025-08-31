using csharp_exception_global_handling.DTOs;
using csharp_exception_global_handling.Models;

namespace csharp_exception_global_handling.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User> UpdateUserBalanceAsync(int id, UpdateUserBalanceRequest request);
        Task DeleteUserAsync(int id);
    }
}
