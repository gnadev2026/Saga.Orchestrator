using Newtonsoft.Json;
using Saga.Orchestrator.Controllers;
using System.Text;

namespace Saga.Orchestrator
{
    public class InventoryProxy(IHttpClientFactory httpClientFactory) : IInventoryProxy
    {
        public async Task<(int, bool)> UpdateInventoryAsync(Order order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);

                var inventoryClient = httpClientFactory.CreateClient("Inventory");
                var inventoryResponse = await inventoryClient.PostAsync("/api/inventory",
                    new StringContent(request, Encoding.UTF8, "application/json"));
                if (inventoryResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(inventoryResponse.ReasonPhrase);
                }
                var inventoryId = await inventoryResponse.Content.ReadAsStringAsync();

                return (Convert.ToInt32(inventoryId), true);
            }
            catch
            {

                return (-1, false);
            }
        }

        public async Task DeleteInventory(int orderId)
        {
            var inventoryClient = httpClientFactory.CreateClient("Inventory");
            var inventoryResponse = await inventoryClient.DeleteAsync($"/api/inventory/{orderId}");
            Console.WriteLine("Inventory Deleted!!");
        }

    }
}
