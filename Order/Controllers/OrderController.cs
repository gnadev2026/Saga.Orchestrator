using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // POST api/<NotifierController>
        [HttpPost]
        public int Post([FromBody] Order order)
        {
            Console.WriteLine($"Created new order   for : {order.ProductName}");
            return 1;
        }

        // DELETE api/<NotifierController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Deleted order : {id}");
        }
    }
}
