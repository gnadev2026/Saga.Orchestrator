namespace Saga.Orchestrator.Controllers
{
    public class OrderResponse
    {
        public bool  Success { get; set; }
        public string? OrderId { get; set; }

        public string? Reason { get; set; } 
    }
}