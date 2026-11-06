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
}

