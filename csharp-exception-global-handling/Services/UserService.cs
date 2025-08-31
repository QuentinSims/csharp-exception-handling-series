using csharp_exception_global_handling.DTOs;
using csharp_exception_global_handling.Exceptions;
using csharp_exception_global_handling.Models;
using csharp_exception_global_handling.Repositories;

namespace csharp_exception_global_handling.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("User ID must be positive", nameof(id));

            _logger.LogDebug("Getting user {UserId}", id);

            var user = await _repository.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", id);
                throw new UserNotFoundException(id);
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            _logger.LogDebug("Getting user by email {Email}", email);

            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
                throw new KeyNotFoundException($"User with email '{email}' was not found");

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogDebug("Getting all users");
            return await _repository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required", nameof(request.Name));

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required", nameof(request.Email));

            if (request.InitialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(request.InitialBalance), "Initial balance cannot be negative");

            // Check for duplicate email
            var existingUser = await _repository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new DuplicateEmailException(request.Email);

            _logger.LogInformation("Creating new user with email {Email}", request.Email);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Balance = request.InitialBalance
            };

            return await _repository.CreateAsync(user);
        }

        public async Task<User> UpdateUserBalanceAsync(int id, UpdateUserBalanceRequest request)
        {
            if (request.NewBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(request.NewBalance), "Balance cannot be negative");

            var user = await GetUserByIdAsync(id); // This will throw UserNotFoundException if not found

            _logger.LogInformation("Updating balance for user {UserId} from {OldBalance} to {NewBalance}",
                id, user.Balance, request.NewBalance);

            user.Balance = request.NewBalance;
            return await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id); // This will throw UserNotFoundException if not found

            _logger.LogInformation("Deleting user {UserId}", id);
            await _repository.DeleteAsync(id);
        }
    }
}
