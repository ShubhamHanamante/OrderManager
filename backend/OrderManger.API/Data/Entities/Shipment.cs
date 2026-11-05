using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public string Carrier { get; set; } = null!;

    public string? TrackingNumber { get; set; }

    public string ShipmentStatus { get; set; } = null!;

    public string? TrackingUrl { get; set; }

    public string? ShippedAtUtc { get; set; }

    public string? EstimatedDeliveryUtc { get; set; }

    public string? DeliveredAtUtc { get; set; }

    public string CreatedAtUtc { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
