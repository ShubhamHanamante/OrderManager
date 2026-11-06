using OrderManager.API.Data.Entities;
using OrderManager.API.DTOs.Orders;
using OrderManager.API.Repositories.Interfaces;
using OrderManager.API.Services.Interfaces;

namespace OrderManager.API.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDetailResponseDto?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, customerId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        return MapToOrderDetailResponseDto(order);
    }

    public async Task<IReadOnlyList<OrderSummaryResponseDto>> GetRecentOrdersAsync(
        int customerId,
        int limit = 5,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersAsync(customerId, status: null, limit: limit, cancellationToken);
        return orders.Select(MapToOrderSummaryResponseDto).ToList();
    }

    public async Task<IReadOnlyList<OrderSummaryResponseDto>> GetOrdersByStatusAsync(
        int customerId,
        string status,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersAsync(customerId, status: status, limit: null, cancellationToken);
        return orders.Select(MapToOrderSummaryResponseDto).ToList();
    }

    public async Task<IReadOnlyList<OrderItemDto>?> GetOrderItemsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        // Verify the order exists and belongs to the customer
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, customerId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var items = await _orderRepository.GetOrderItemsAsync(orderNumber, customerId, cancellationToken);
        return items.Select(item => new OrderItemDto
        {
            OrderItemId = item.OrderItemId,
            ProductId = item.ProductId,
            ProductName = item.ProductNameSnapshot,
            Sku = item.Sku,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPricePaise / 100.0m,
            UnitPricePaise = item.UnitPricePaise,
            Discount = item.DiscountPaise / 100.0m,
            DiscountPaise = item.DiscountPaise,
            LineTotal = item.LineTotalPaise / 100.0m,
            LineTotalPaise = item.LineTotalPaise
        }).ToList();
    }

    private static OrderDetailResponseDto MapToOrderDetailResponseDto(Order order)
    {
        var customerName = order.Customer != null
            ? $"{order.Customer.FirstName} {order.Customer.LastName}".Trim()
            : string.Empty;

        return new OrderDetailResponseDto
        {
            OrderNumber = order.OrderNumber,
            CustomerName = customerName,
            OrderDateUtc = order.OrderDateUtc,
            OrderStatus = order.OrderStatus,
            Currency = order.Currency,
            ShippingAddress = order.ShippingAddress,
            Subtotal = order.SubtotalPaise / 100.0m,
            SubtotalPaise = order.SubtotalPaise,
            Discount = order.DiscountPaise / 100.0m,
            DiscountPaise = order.DiscountPaise,
            Shipping = order.ShippingPaise / 100.0m,
            ShippingPaise = order.ShippingPaise,
            Tax = order.TaxPaise / 100.0m,
            TaxPaise = order.TaxPaise,
            Total = order.TotalPaise / 100.0m,
            TotalPaise = order.TotalPaise,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                OrderItemId = item.OrderItemId,
                ProductId = item.ProductId,
                ProductName = item.ProductNameSnapshot,
                Sku = item.Sku,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPricePaise / 100.0m,
                UnitPricePaise = item.UnitPricePaise,
                Discount = item.DiscountPaise / 100.0m,
                DiscountPaise = item.DiscountPaise,
                LineTotal = item.LineTotalPaise / 100.0m,
                LineTotalPaise = item.LineTotalPaise
            }).ToList(),
            Payments = order.Payments.Select(payment => new PaymentDto
            {
                PaymentId = payment.PaymentId,
                PaymentReference = payment.PaymentReference,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                Amount = payment.AmountPaise / 100.0m,
                AmountPaise = payment.AmountPaise,
                Currency = payment.Currency,
                PaidAtUtc = payment.PaidAtUtc
            }).ToList(),
            Shipments = order.Shipments.Select(shipment => new ShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                Carrier = shipment.Carrier,
                TrackingNumber = shipment.TrackingNumber,
                ShipmentStatus = shipment.ShipmentStatus,
                TrackingUrl = shipment.TrackingUrl,
                ShippedAtUtc = shipment.ShippedAtUtc,
                EstimatedDeliveryUtc = shipment.EstimatedDeliveryUtc,
                DeliveredAtUtc = shipment.DeliveredAtUtc
            }).ToList(),
            StatusHistory = order.OrderStatusHistories
                .OrderBy(history => history.ChangedAtUtc)
                .Select(history => new OrderStatusHistoryDto
                {
                    HistoryId = history.HistoryId,
                    Status = history.Status,
                    StatusDescription = history.StatusDescription,
                    ChangedBy = history.ChangedBy,
                    ChangedAtUtc = history.ChangedAtUtc
                }).ToList()
        };
    }

    private static OrderSummaryResponseDto MapToOrderSummaryResponseDto(Order order)
    {
        // Pick the latest shipment (if any) for summary display
        var latestShipment = order.Shipments
            .OrderByDescending(s => s.ShippedAtUtc ?? s.CreatedAtUtc)
            .FirstOrDefault();

        return new OrderSummaryResponseDto
        {
            OrderNumber = order.OrderNumber,
            OrderDateUtc = order.OrderDateUtc,
            OrderStatus = order.OrderStatus,
            Currency = order.Currency,
            Total = order.TotalPaise / 100.0m,
            TotalPaise = order.TotalPaise,
            ItemCount = order.OrderItems.Count,
            Carrier = latestShipment?.Carrier,
            TrackingNumber = latestShipment?.TrackingNumber,
            ShipmentStatus = latestShipment?.ShipmentStatus,
            EstimatedDeliveryUtc = latestShipment?.EstimatedDeliveryUtc
        };
    }
}

