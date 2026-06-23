using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController(IOrderManager  orderManager) : ControllerBase
    {
        [HttpPost]
        public  OrderResponse Post([FromBody] Order order)
        {
            var response = orderManager.CreateOrder(order);
            return new OrderResponse { Success = response };
        }
    }
}
