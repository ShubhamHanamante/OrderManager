using Microsoft.EntityFrameworkCore;
using OrderManager.API.Data;
using OrderManager.API.Repositories;
using OrderManager.API.Repositories.Interfaces;
using OrderManager.API.Services;
using OrderManager.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var databasePath = builder.Configuration["Database:Path"]
    ?? throw new InvalidOperationException("Database path is missing.");

var fullDatabasePath = Path.GetFullPath(
    databasePath,
    builder.Environment.ContentRootPath
);

builder.Services.AddDbContext<OrderManagerDbContext>(options =>
    options.UseSqlite($"Data Source={fullDatabasePath};Foreign Keys=True")
);

// Register application repositories and services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
