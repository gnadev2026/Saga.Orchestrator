using Saga.Orchestrator.Controllers;

namespace Saga.Orchestrator
{
    public interface IOrderManager
    {
        bool CreateOrder(Order input);
    }
}