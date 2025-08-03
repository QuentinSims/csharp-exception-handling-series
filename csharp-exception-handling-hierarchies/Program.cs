using csharp_exception_handling_hierarchies.Data;
using csharp_exception_handling_hierarchies.Models;
using csharp_exception_handling_hierarchies.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ExceptionHierarchiesDb"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPricingService, PricingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void SeedData(AppDbContext context)
{
    context.Products.AddRange(
        new Product { Id = 1, Name = "Gaming Laptop", Price = 1299.99m, StockQuantity = 5 },
        new Product { Id = 2, Name = "Wireless Mouse", Price = 49.99m, StockQuantity = 0 },
        new Product { Id = 3, Name = "Mechanical Keyboard", Price = 129.99m, StockQuantity = 15 }
    );

    context.Customers.AddRange(
        new Customer { Id = 1, Name = "John Doe", Email = "john@example.com", IsActive = true },
        new Customer { Id = 2, Name = "Jane Smith", Email = "jane@example.com", IsActive = false }
    );

    context.SaveChanges();
}