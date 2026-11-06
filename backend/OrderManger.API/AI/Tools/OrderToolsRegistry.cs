using System.Text.Json;
using OrderManager.API.AI.Ollama;
using OrderManager.API.Services.Interfaces;

namespace OrderManager.API.AI.Tools;

public interface IOrderToolsRegistry
{
    List<OllamaTool> GetToolDefinitions();
    Task<ToolExecutionResult> ExecuteToolAsync(
        string toolName,
        JsonElement arguments,
        int customerId,
        CancellationToken cancellationToken = default);
}

public class ToolExecutionResult
{
    public string ToolName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool Success { get; set; }
    public object? StructuredData { get; set; }
}

public class OrderToolsRegistry : IOrderToolsRegistry
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderToolsRegistry> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

    public OrderToolsRegistry(IOrderService orderService, ILogger<OrderToolsRegistry> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public List<OllamaTool> GetToolDefinitions()
    {
        return
        [
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_order_details",
                    Description = "Retrieve complete details for a specific order including status, total amounts, shipping address, products, payment status, and delivery tracking.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderNumber = new
                            {
                                type = "string",
                                description = "The unique order number (e.g. ORD-2026-1002)"
                            }
                        },
                        required = new[] { "orderNumber" }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_recent_orders",
                    Description = "Retrieve the customer's recent orders list ordered with newest orders first.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            limit = new
                            {
                                type = "integer",
                                description = "Maximum number of recent orders to retrieve (default is 5)"
                            }
                        }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_orders_by_status",
                    Description = "Retrieve orders filtered by their status (e.g. Pending, Confirmed, Processing, Shipped, Delivered, Cancelled).",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            status = new
                            {
                                type = "string",
                                description = "The order status to filter by (e.g. 'Shipped', 'Delivered', 'Pending', 'Processing', 'Cancelled')"
                            }
                        },
                        required = new[] { "status" }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_order_items",
                    Description = "Retrieve the list of products/items, quantities, and prices for a specific order.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderNumber = new
                            {
                                type = "string",
                                description = "The order number (e.g. ORD-2026-1002)"
                            }
                        },
                        required = new[] { "orderNumber" }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_payment_details",
                    Description = "Retrieve payment transaction details, payment method, payment status, and amount paid for an order.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderNumber = new
                            {
                                type = "string",
                                description = "The order number (e.g. ORD-2026-1002)"
                            }
                        },
                        required = new[] { "orderNumber" }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_shipment_tracking",
                    Description = "Retrieve shipment tracking information, delivery carrier, tracking number, tracking URL, and estimated delivery date for an order.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderNumber = new
                            {
                                type = "string",
                                description = "The order number (e.g. ORD-2026-1002)"
                            }
                        },
                        required = new[] { "orderNumber" }
                    }
                }
            },
            new OllamaTool
            {
                Type = "function",
                Function = new OllamaFunction
                {
                    Name = "get_order_history",
                    Description = "Retrieve the chronological history of order status changes and descriptions.",
                    Parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderNumber = new
                            {
                                type = "string",
                                description = "The order number (e.g. ORD-2026-1002)"
                            }
                        },
                        required = new[] { "orderNumber" }
                    }
                }
            }
        ];
    }

    public async Task<ToolExecutionResult> ExecuteToolAsync(
        string toolName,
        JsonElement arguments,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing tool {ToolName} for customer {CustomerId}", toolName, customerId);

        try
        {
            switch (toolName.ToLowerInvariant())
            {
                case "get_order_details":
                {
                    var orderNumber = GetStringArg(arguments, "orderNumber");
                    if (string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return Failure(toolName, "Missing required argument 'orderNumber'.");
                    }

                    var order = await _orderService.GetByOrderNumberAsync(orderNumber, customerId, cancellationToken);
                    if (order is null)
                    {
                        return Failure(toolName, $"Order '{orderNumber}' was not found for this customer.");
                    }

                    return Success(toolName, order, order);
                }

                case "get_recent_orders":
                {
                    var limit = GetIntArg(arguments, "limit") ?? 5;
                    var orders = await _orderService.GetRecentOrdersAsync(customerId, limit, cancellationToken);
                    return Success(toolName, orders, orders);
                }

                case "get_orders_by_status":
                {
                    var status = GetStringArg(arguments, "status");
                    if (string.IsNullOrWhiteSpace(status))
                    {
                        return Failure(toolName, "Missing required argument 'status'.");
                    }

                    var orders = await _orderService.GetOrdersByStatusAsync(customerId, status, cancellationToken);
                    return Success(toolName, orders, orders);
                }

                case "get_order_items":
                {
                    var orderNumber = GetStringArg(arguments, "orderNumber");
                    if (string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return Failure(toolName, "Missing required argument 'orderNumber'.");
                    }

                    var items = await _orderService.GetOrderItemsAsync(orderNumber, customerId, cancellationToken);
                    if (items is null)
                    {
                        return Failure(toolName, $"Order '{orderNumber}' was not found for this customer.");
                    }

                    return Success(toolName, items, items);
                }

                case "get_payment_details":
                {
                    var orderNumber = GetStringArg(arguments, "orderNumber");
                    if (string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return Failure(toolName, "Missing required argument 'orderNumber'.");
                    }

                    var payments = await _orderService.GetPaymentsAsync(orderNumber, customerId, cancellationToken);
                    if (payments is null)
                    {
                        return Failure(toolName, $"Order '{orderNumber}' was not found for this customer.");
                    }

                    return Success(toolName, payments, payments);
                }

                case "get_shipment_tracking":
                {
                    var orderNumber = GetStringArg(arguments, "orderNumber");
                    if (string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return Failure(toolName, "Missing required argument 'orderNumber'.");
                    }

                    var shipments = await _orderService.GetShipmentsAsync(orderNumber, customerId, cancellationToken);
                    if (shipments is null)
                    {
                        return Failure(toolName, $"Order '{orderNumber}' was not found for this customer.");
                    }

                    return Success(toolName, shipments, shipments);
                }

                case "get_order_history":
                {
                    var orderNumber = GetStringArg(arguments, "orderNumber");
                    if (string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return Failure(toolName, "Missing required argument 'orderNumber'.");
                    }

                    var history = await _orderService.GetStatusHistoryAsync(orderNumber, customerId, cancellationToken);
                    if (history is null)
                    {
                        return Failure(toolName, $"Order '{orderNumber}' was not found for this customer.");
                    }

                    return Success(toolName, history, history);
                }

                default:
                    return Failure(toolName, $"Unknown tool '{toolName}'.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing tool {ToolName} for customer {CustomerId}", toolName, customerId);
            return Failure(toolName, $"Error executing tool: {ex.Message}");
        }
    }

    private static string? GetStringArg(JsonElement arguments, string propertyName)
    {
        if (arguments.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var prop in arguments.EnumerateObject())
        {
            if (string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                return prop.Value.GetString();
            }
        }

        return null;
    }

    private static int? GetIntArg(JsonElement arguments, string propertyName)
    {
        if (arguments.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var prop in arguments.EnumerateObject())
        {
            if (string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                if (prop.Value.TryGetInt32(out var val))
                {
                    return val;
                }

                if (int.TryParse(prop.Value.GetString(), out var parsed))
                {
                    return parsed;
                }
            }
        }

        return null;
    }

    private static ToolExecutionResult Success(string toolName, object data, object? structuredData)
    {
        return new ToolExecutionResult
        {
            ToolName = toolName,
            Content = JsonSerializer.Serialize(data, JsonOptions),
            Success = true,
            StructuredData = structuredData
        };
    }

    private static ToolExecutionResult Failure(string toolName, string error)
    {
        return new ToolExecutionResult
        {
            ToolName = toolName,
            Content = JsonSerializer.Serialize(new { error }, JsonOptions),
            Success = false,
            StructuredData = null
        };
    }
}

