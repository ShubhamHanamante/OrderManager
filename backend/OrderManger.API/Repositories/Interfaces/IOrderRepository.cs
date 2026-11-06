using OrderManager.API.Data.Entities;

namespace OrderManager.API.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        int customerId,
        CancellationToken cancellationToken = default);
}
