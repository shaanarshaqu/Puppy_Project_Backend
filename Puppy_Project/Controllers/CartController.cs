using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models.Cart;
using Puppy_Project.Services.Cart;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart _cart;
        public CartController(ICart cart, PuppyDb puppyDb) 
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

    }
}
