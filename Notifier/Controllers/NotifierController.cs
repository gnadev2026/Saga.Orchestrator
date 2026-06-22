using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifierController : ControllerBase
    {
        // POST api/<NotifierController>
        [HttpPost]
        public int Post([FromBody] Notifier notifier)
        {
            Console.WriteLine($"Sent  notification for : {notifier.ProductName}");
            return 3;
        }

        // DELETE api/<NotifierController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Sent rollback transactioin  notification : {id}");
        }
    }
}
