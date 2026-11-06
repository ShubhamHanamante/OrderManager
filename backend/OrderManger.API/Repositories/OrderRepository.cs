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

    public async Task<IReadOnlyList<Order>> GetOrdersAsync(
        int customerId,
        string? status = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.OrderItems)
            .Include(o => o.Shipments)
            .Where(o => o.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.OrderStatus.ToLower() == status.Trim().ToLower());
        }

        query = query.OrderByDescending(o => o.OrderDateUtc);

        if (limit.HasValue && limit.Value > 0)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderItem>> GetOrderItemsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderItems
            .AsNoTracking()
            .Where(oi =>
                oi.Order.OrderNumber == orderNumber &&
                oi.Order.CustomerId == customerId)
            .OrderBy(oi => oi.OrderItemId)
            .ToListAsync(cancellationToken);
    }
}