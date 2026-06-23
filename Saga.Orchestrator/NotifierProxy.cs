using Newtonsoft.Json;
using Saga.Orchestrator.Controllers;
using System.Text;

namespace Saga.Orchestrator
{
    public class NotifierProxy(IHttpClientFactory httpClientFactory) : INotifierProxy
    {
        public async Task<(int, bool)> SendAsync(Order order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);

                var notifierClient = httpClientFactory.CreateClient("Notifier");
                var notifierResponse = await notifierClient.PostAsync("/api/notifier",
                    new StringContent(request, Encoding.UTF8, "application/json"));
                var notifierId = await notifierResponse.Content.ReadAsStringAsync();

                return (Convert.ToInt32(notifierId), true);
            }
            catch
            {

                return (-1, false);
            }
        }

    }
}
