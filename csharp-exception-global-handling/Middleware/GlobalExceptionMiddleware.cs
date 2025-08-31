using csharp_exception_global_handling.Exceptions;
using System.Security.Claims;
using System.Text.Json;

namespace csharp_exception_global_handling.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = context.TraceIdentifier,
                ["RequestPath"] = context.Request.Path,
                ["RequestMethod"] = context.Request.Method,
                ["UserId"] = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous",
                ["UserAgent"] = context.Request.Headers.UserAgent.ToString()
            });

            _logger.LogError(exception,
                "Unhandled exception occurred for {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);

            context.Response.ContentType = "application/json";

            var (statusCode, response) = MapExceptionToResponse(exception, context);

            context.Response.StatusCode = statusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static (int statusCode, object response) MapExceptionToResponse(Exception exception, HttpContext context)
        {
            var isDevelopment = context.RequestServices
                .GetService<IWebHostEnvironment>()?
                .IsDevelopment() == true;

            return exception switch
            {
                UserNotFoundException ex => (404, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "UserNotFound",
                        Data = new { ex.UserId }
                    }
                }),

                InsufficientFundsException ex => (409, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "InsufficientFunds",
                        Data = new { ex.UserId, ex.CurrentBalance, ex.RequestedAmount }
                    }
                }),

                InvalidProductCodeException ex => (400, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "InvalidProductCode",
                        Data = new { ex.ProductCode }
                    }
                }),

                DuplicateEmailException ex => (409, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "DuplicateEmail",
                        Data = new { ex.Email }
                    }
                }),

                // Built-in .NET exceptions
                ArgumentNullException ex => (400, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "BadRequest",
                        Data = new { Parameter = ex.ParamName }
                    }
                }),

                ArgumentOutOfRangeException ex => (400, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "BadRequest",
                        Data = new { Parameter = ex.ParamName, ex.ActualValue }
                    }
                }),

                ArgumentException ex => (400, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "BadRequest",
                        Data = new { Parameter = ex.ParamName }
                    }
                }),

                KeyNotFoundException ex => (404, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "NotFound"
                    }
                }),

                InvalidOperationException ex => (409, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "Conflict"
                    }
                }),

                FormatException ex => (400, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "BadRequest"
                    }
                }),

                NotSupportedException ex => (501, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "NotImplemented"
                    }
                }),

                UnauthorizedAccessException ex => (401, new
                {
                    Error = new
                    {
                        Message = ex.Message,
                        Type = "Unauthorized"
                    }
                }),

                // Catch-all for unexpected exceptions
                _ => (500, new
                {
                    Error = new
                    {
                        Message = isDevelopment ? exception.Message : "An internal server error occurred",
                        Type = "InternalServerError",
                        RequestId = context.TraceIdentifier,
                        Details = isDevelopment ? exception.StackTrace : null
                    }
                })
            };
        }
    }
}
