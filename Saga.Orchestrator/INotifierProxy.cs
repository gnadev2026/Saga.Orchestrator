using Saga.Orchestrator.Controllers;

namespace Saga.Orchestrator
{
    public interface INotifierProxy
    {
        Task<(int, bool)> SendAsync(Order order);
    }
}