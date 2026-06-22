using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IHttpClientFactory httpClientFactory ) : ControllerBase
    {
        [HttpPost]
        public async Task<OrderResponse> Post([FromBody] Order order)
        {
            var request = JsonConvert.SerializeObject(order);

            //Create Order
            var orderClient = httpClientFactory.CreateClient("Order");
            var orderResponse = await orderClient.PostAsync("api/order",
                new StringContent(request,Encoding.UTF8, "application/JSON"));
            var orderId = await orderResponse.Content.ReadAsStringAsync();


            // Update Inventory 
            var inventoryId = string.Empty;
            try
            {
                var inventoryClient = httpClientFactory.CreateClient("Inventory");
                var inventoryResponse = await inventoryClient.PostAsync("api/inventory",
                    new StringContent(request, Encoding.UTF8, "application/JSON"));
                if (inventoryResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(inventoryResponse.ReasonPhrase);
                }
                inventoryId = await inventoryResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                await orderClient.DeleteAsync($"api/order/{orderId}");
                return new OrderResponse
                {
                    Success = false,
                    Reason = ex.Message
                };
            }

            //Send Notification
            var notifierClient = httpClientFactory.CreateClient("Notifier");
            var notifierResponse =   await notifierClient.PostAsync("api/notifier",
                new StringContent(request, Encoding.UTF8, "application/JSON"));
            var notifierId = await notifierResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"Order Id : {orderId}, Inventory Id : {inventoryId}, notifierId : {notifierId}");

            return new OrderResponse { OrderId = orderId, Success = true };
        }
    }
}
