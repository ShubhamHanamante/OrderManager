namespace OrderManager.API.DTOs.Orders;

public class OrderDetailResponseDto
{
    public string OrderNumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string OrderDateUtc { get; set; } = null!;
    public string OrderStatus { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;

    public decimal Subtotal { get; set; }
    public int SubtotalPaise { get; set; }
    public decimal Discount { get; set; }
    public int DiscountPaise { get; set; }
    public decimal Shipping { get; set; }
    public int ShippingPaise { get; set; }
    public decimal Tax { get; set; }
    public int TaxPaise { get; set; }
    public decimal Total { get; set; }
    public int TotalPaise { get; set; }

    public List<OrderItemDto> Items { get; set; } = [];
    public List<PaymentDto> Payments { get; set; } = [];
    public List<ShipmentDto> Shipments { get; set; } = [];
    public List<OrderStatusHistoryDto> StatusHistory { get; set; } = [];
}

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string Sku { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitPricePaise { get; set; }
    public decimal Discount { get; set; }
    public int DiscountPaise { get; set; }
    public decimal LineTotal { get; set; }
    public int LineTotalPaise { get; set; }
}

public class PaymentDto
{
    public int PaymentId { get; set; }
    public string PaymentReference { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public decimal Amount { get; set; }
    public int AmountPaise { get; set; }
    public string Currency { get; set; } = null!;
    public string? PaidAtUtc { get; set; }
}

public class ShipmentDto
{
    public int ShipmentId { get; set; }
    public string Carrier { get; set; } = null!;
    public string? TrackingNumber { get; set; }
    public string ShipmentStatus { get; set; } = null!;
    public string? TrackingUrl { get; set; }
    public string? ShippedAtUtc { get; set; }
    public string? EstimatedDeliveryUtc { get; set; }
    public string? DeliveredAtUtc { get; set; }
}

public class OrderStatusHistoryDto
{
    public int HistoryId { get; set; }
    public string Status { get; set; } = null!;
    public string? StatusDescription { get; set; }
    public string ChangedBy { get; set; } = null!;
    public string ChangedAtUtc { get; set; } = null!;
}

/// <summary>
/// Lightweight order summary for list endpoints (recent orders, orders by status).
/// Does not include full items, payments, or status history.
/// </summary>
public class OrderSummaryResponseDto
{
    public string OrderNumber { get; set; } = null!;
    public string OrderDateUtc { get; set; } = null!;
    public string OrderStatus { get; set; } = null!;
    public string Currency { get; set; } = null!;

    public decimal Total { get; set; }
    public int TotalPaise { get; set; }

    public int ItemCount { get; set; }

    // Latest shipment info (if any)
    public string? Carrier { get; set; }
    public string? TrackingNumber { get; set; }
    public string? ShipmentStatus { get; set; }
    public string? EstimatedDeliveryUtc { get; set; }
}
