using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public string OrderDateUtc { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public int SubtotalPaise { get; set; }

    public int DiscountPaise { get; set; }

    public int ShippingPaise { get; set; }

    public int TaxPaise { get; set; }

    public int TotalPaise { get; set; }

    public string Currency { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string CreatedAtUtc { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
