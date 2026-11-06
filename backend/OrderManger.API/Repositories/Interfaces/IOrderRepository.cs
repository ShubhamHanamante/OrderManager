using OrderManager.API.Data.Entities;

namespace OrderManager.API.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetOrdersAsync(
        int customerId,
        string? status = null,
        int? limit = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderItem>> GetOrderItemsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Payment>> GetPaymentsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shipment>> GetShipmentsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderStatusHistory>> GetStatusHistoryAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);
}
