using Microsoft.EntityFrameworkCore;
using OrderManager.API.AI.ChatOrchestration;
using OrderManager.API.AI.Ollama;
using OrderManager.API.AI.Tools;
using OrderManager.API.Data;
using OrderManager.API.Repositories;
using OrderManager.API.Repositories.Interfaces;
using OrderManager.API.Services;
using OrderManager.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5078")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true); // Allow any local development origin
    });
});

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

// Register Ollama AI client and tools
builder.Services.AddHttpClient<IOllamaClient, OllamaClient>(client =>
{
    var endpoint = builder.Configuration["Ollama:Endpoint"] ?? "http://localhost:11434";
    client.BaseAddress = new Uri(endpoint);
    var timeoutSec = builder.Configuration.GetValue<int>("Ollama:TimeoutSeconds", 60);
    client.Timeout = TimeSpan.FromSeconds(timeoutSec);
});

builder.Services.AddScoped<IOrderToolsRegistry, OrderToolsRegistry>();
builder.Services.AddScoped<IChatOrchestrationService, ChatOrchestrationService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
