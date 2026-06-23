using Saga.Orchestrator.Controllers;

namespace Saga.Orchestrator
{
    public interface IOrderProxy
    {
        Task<(int, bool)> CreateOrderAsync(Order order);

        Task DeleteOrder(int orderId);
    }
}