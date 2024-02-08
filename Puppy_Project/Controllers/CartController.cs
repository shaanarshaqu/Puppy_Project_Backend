using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models.CartDTO;
using Puppy_Project.Services.Carts;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cart;
        public CartController(ICartService cart, PuppyDb puppyDb) 
        {
            _cart= cart;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCartDetails(int id)
        {
            var usercart = _cart.ListCartofUsers(id);
            return usercart==null ? BadRequest("User Has no cart") : Ok(usercart);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddUserCart([FromBody] AddCartDTO cartitem)
        {
            bool isAdded = _cart.CreateUserCart(cartitem) ;
            if (!isAdded)
            {
                return BadRequest();
            }
            return Ok(isAdded);
        }

        [HttpPut("increment/{id:int}")]
        public IActionResult IncrementCartitem(int id)
        {
            bool incremented = _cart.UserCartQtyIncrement(id);
            return incremented ? Ok("success") : BadRequest();
        }

        [HttpPut("decrement/{id:int}")]
        public IActionResult DecrementCartitem(int id)
        {
            bool decremented = _cart.UserCartQtyDecrement(id);
            return decremented ? Ok("success") : BadRequest();
        }

        [HttpDelete("{id:int}")]
        public IActionResult RemoveFromCart(int id)
        {
            bool isRemoved = _cart.RemoveFromUserCart(id);
            return isRemoved ? Ok("success") : NotFound();
        }

    }
}
