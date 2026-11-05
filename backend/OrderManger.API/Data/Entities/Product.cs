using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public int UnitPricePaise { get; set; }

    public int StockQuantity { get; set; }

    public int IsActive { get; set; }

    public string CreatedAtUtc { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
