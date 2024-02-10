using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> ListUserOrder(int id)
        {
            try
            {
                var order_list = await _order.ListUserOrder(id);
                return Ok(order_list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(inputOrderDTO order)
        {
            try
            {
                bool isAdded = await _order.AddUserOrder(order);
                return isAdded ? Ok(isAdded) : BadRequest();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                bool isDeleted = await _order.RemoveAllorders(id);
                return isDeleted ? Ok(isDeleted) : BadRequest(isDeleted);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
