using OrderManager.API.DTOs.Orders;

namespace OrderManager.API.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDetailResponseDto?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderSummaryResponseDto>> GetRecentOrdersAsync(
        int customerId,
        int limit = 5,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderSummaryResponseDto>> GetOrdersByStatusAsync(
        int customerId,
        string status,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderItemDto>?> GetOrderItemsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PaymentDto>?> GetPaymentsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShipmentDto>?> GetShipmentsAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderStatusHistoryDto>?> GetStatusHistoryAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);
}
