using System;
using System.Collections.Generic;

namespace OrderManager.API.Data.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public int IsActive { get; set; }

    public string CreatedAtUtc { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
