using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Services.Orders;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _order;
        public OrderController(IOrderService order) 
        {
            _order = order;
        }



        [HttpGet("{id:int}")]
        public IActionResult ListUserOrder(int id)
        {
            var order_list = _order.ListUserOrder(id);
            return Ok(order_list);
        }

        [HttpPost]
        public IActionResult CreateOrder(inputOrderDTO order)
        {
            bool isAdded = _order.AddUserOrder(order);
            return isAdded ? Ok(isAdded) : BadRequest();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOrder(int id)
        {
            bool isDeleted = _order.RemoveAllorders(id);
            return isDeleted ? Ok(isDeleted):BadRequest(isDeleted);
        }
    }
}
