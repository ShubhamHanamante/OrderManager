using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class OrderStatusHistory
{
    public int HistoryId { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; } = null!;

    public string? StatusDescription { get; set; }

    public string ChangedBy { get; set; } = null!;

    public string ChangedAtUtc { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
