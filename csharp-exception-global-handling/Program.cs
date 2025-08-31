using csharp_exception_global_handling.Data;
using csharp_exception_global_handling.Middleware;
using csharp_exception_global_handling.Models;
using csharp_exception_global_handling.Repositories;
using csharp_exception_global_handling.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ExceptionDemoDb"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedDataAsync(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task SeedDataAsync(AppDbContext context)
{
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", Balance = 1000.00m },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Balance = 500.00m }
        );
        await context.SaveChangesAsync();
    }
}


