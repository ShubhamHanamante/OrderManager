using Microsoft.AspNetCore.Mvc;
using OrderManager.API.DTOs.Orders;
using OrderManager.API.Services.Interfaces;

namespace OrderManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets order details by order number for a customer.
    /// Note: customerId query parameter is temporary for development testing
    /// and must be replaced with authenticated customer context in production.
    /// </summary>
    [HttpGet("{orderNumber}")]
    [ProducesResponseType(typeof(OrderDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDetailResponseDto>> GetByOrderNumber(
        string orderNumber,
        [FromQuery] int customerId, // [Development-Only]: Replace with authenticated customer identity
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return BadRequest(new { message = "Order number cannot be empty." });
        }

        if (customerId <= 0)
        {
            return BadRequest(new { message = "A valid customerId query parameter is required for development testing." });
        }

        var order = await _orderService.GetByOrderNumberAsync(orderNumber, customerId, cancellationToken);
        if (order is null)
        {
            return NotFound(new { message = $"Order '{orderNumber}' was not found for customer {customerId}." });
        }

        return Ok(order);
    }

    /// <summary>
    /// Gets recent orders for a customer (most recent first).
    /// </summary>
    [HttpGet("recent")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<OrderSummaryResponseDto>>> GetRecentOrders(
        [FromQuery] int customerId, // [Development-Only]: Replace with authenticated customer identity
        [FromQuery] int limit = 5,
        CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            return BadRequest(new { message = "A valid customerId query parameter is required for development testing." });
        }

        if (limit <= 0 || limit > 50)
        {
            return BadRequest(new { message = "Limit must be between 1 and 50." });
        }

        var orders = await _orderService.GetRecentOrdersAsync(customerId, limit, cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Gets orders filtered by status for a customer.
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<OrderSummaryResponseDto>>> GetOrdersByStatus(
        string status,
        [FromQuery] int customerId, // [Development-Only]: Replace with authenticated customer identity
        CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            return BadRequest(new { message = "A valid customerId query parameter is required for development testing." });
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            return BadRequest(new { message = "Order status cannot be empty." });
        }

        var orders = await _orderService.GetOrdersByStatusAsync(customerId, status, cancellationToken);
        return Ok(orders);
    }

    /// <summary>
    /// Gets the list of items for a specific order.
    /// </summary>
    [HttpGet("{orderNumber}/items")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<OrderItemDto>>> GetOrderItems(
        string orderNumber,
        [FromQuery] int customerId, // [Development-Only]: Replace with authenticated customer identity
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return BadRequest(new { message = "Order number cannot be empty." });
        }

        if (customerId <= 0)
        {
            return BadRequest(new { message = "A valid customerId query parameter is required for development testing." });
        }

        var items = await _orderService.GetOrderItemsAsync(orderNumber, customerId, cancellationToken);
        if (items is null)
        {
            return NotFound(new { message = $"Order '{orderNumber}' was not found for customer {customerId}." });
        }

        return Ok(items);
    }
}

