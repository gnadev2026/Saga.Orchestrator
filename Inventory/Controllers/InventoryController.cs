using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        // POST api/<InventoryController>
        [HttpPost]
        public int Post([FromBody] Inventory inventory)
        {
            throw new Exception("Error Occured...while updating Inventory.");
            Console.WriteLine($"Updated  Inventory for : {inventory.ProductName}");
            return 2;
        }


        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Deleted inventory : {id}");
        }
    }
}
