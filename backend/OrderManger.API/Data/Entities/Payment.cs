using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentReference { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public int AmountPaise { get; set; }

    public string Currency { get; set; } = null!;

    public string? PaidAtUtc { get; set; }

    public string CreatedAtUtc { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
