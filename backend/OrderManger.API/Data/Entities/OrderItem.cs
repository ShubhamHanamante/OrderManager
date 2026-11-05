using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductNameSnapshot { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    public int UnitPricePaise { get; set; }

    public int DiscountPaise { get; set; }

    public int LineTotalPaise { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
