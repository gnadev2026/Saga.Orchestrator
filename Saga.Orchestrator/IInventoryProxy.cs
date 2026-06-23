using Saga.Orchestrator.Controllers;

namespace Saga.Orchestrator
{
    public interface IInventoryProxy
    {
        Task<(int, bool)> UpdateInventoryAsync(Order order);

        Task DeleteInventory(int orderId);
    }
}