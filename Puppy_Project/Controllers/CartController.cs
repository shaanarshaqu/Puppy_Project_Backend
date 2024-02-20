using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models.CartDTO;
using Puppy_Project.Models.ProductDTO;
using Puppy_Project.Secure;
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

        [HttpGet("UserCart")]
        [Authorize]
        public async Task<IActionResult> GetUserCart()
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int id = TokenDecoder.DecodeToken(token);
            if (id == -1)
            {
                return BadRequest();
            }
            var usercart = await _cart.ListCartofUsers(id);
            return usercart==null ? BadRequest("User Has no cart") : Ok(usercart);
        }

        [HttpPost("AddUserCart")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUserCart([FromBody] AddCartDTO cartitem)
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (TokenDecoder.DecodeToken(token.Replace("Bearer ", "")) != cartitem.UserId)
            {
                return StatusCode(StatusCodes.Status203NonAuthoritative);
            }
            bool isAdded = await _cart.CreateUserCart(cartitem) ;
            if (!isAdded)
            {
                return BadRequest();
            }
            return Ok(isAdded);
        }

        [HttpPut("increment")]
        [Authorize]
        public async Task<IActionResult> IncrementCartitem([FromBody] QtyControllDto item)
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int id = TokenDecoder.DecodeToken(token);
            if (id == -1 || item.UserId != id)
            {
                return BadRequest();
            }
            bool incremented = await _cart.UserCartQtyIncrement(item.ElementId);
            return incremented ? Ok("success") : BadRequest();
        }

        [HttpPut("decrement")]
        [Authorize]
        public async Task<IActionResult> DecrementCartitem([FromBody] QtyControllDto item)
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int id = TokenDecoder.DecodeToken(token);
            if (id == -1 || item.UserId != id)
            {
                return BadRequest();
            }
            bool decremented = await _cart.UserCartQtyDecrement(item.ElementId);
            return decremented ? Ok("success") : BadRequest();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart([FromBody] QtyControllDto item)
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int id = TokenDecoder.DecodeToken(token);
            if (id == -1 || item.UserId != id)
            {
                return BadRequest();
            }
            bool isRemoved = await _cart.RemoveFromUserCart(item.ElementId);
            return isRemoved ? Ok("success") : NotFound();
        }

    }
}
