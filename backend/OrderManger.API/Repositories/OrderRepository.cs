using Microsoft.EntityFrameworkCore;
using OrderManager.API.Data;
using OrderManager.API.Data.Entities;
using OrderManager.API.Repositories.Interfaces;

namespace OrderManager.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderManagerDbContext _context;

    public OrderRepository(OrderManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
                   .AsNoTracking()
                   .AsSplitQuery()
                   .Include(o => o.Customer)
                   .Include(o => o.OrderItems)
                   .Include(o => o.Payments)
                   .Include(o => o.Shipments)
                   .Include(o => o.OrderStatusHistories)
                   .Where(o =>
                       o.OrderNumber == orderNumber &&
                       o.CustomerId == customerId)
                   .SingleOrDefaultAsync(cancellationToken);
    }
}
