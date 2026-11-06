using OrderManager.API.DTOs.Orders;

namespace OrderManager.API.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDetailResponseDto?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);
}
