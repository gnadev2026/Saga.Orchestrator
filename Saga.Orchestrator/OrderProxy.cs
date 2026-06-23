using Newtonsoft.Json;
using Saga.Orchestrator.Controllers;
using System.Text;

namespace Saga.Orchestrator
{
    public class OrderProxy(IHttpClientFactory httpClientFactory) : IOrderProxy
    {
        public async Task<(int, bool)> CreateOrderAsync(Order order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);

                var orderClient = httpClientFactory.CreateClient("Order");
                var orderResponse = await orderClient.PostAsync("/api/order",
                    new StringContent(request, Encoding.UTF8, "application/json"));
                var orderId = await orderResponse.Content.ReadAsStringAsync();

                return (Convert.ToInt32(orderId), true);
            }
            catch
            {

                return (-1, false);
            }
        }

        public async Task DeleteOrder(int orderId)
        {
            var orderClient = httpClientFactory.CreateClient("Order");
            var response = await orderClient.DeleteAsync($"/api/order/{orderId}");
            Console.WriteLine("Order Deleted!!");
        }

    }
}
