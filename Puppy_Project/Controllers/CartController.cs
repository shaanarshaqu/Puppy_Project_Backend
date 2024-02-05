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
        private readonly PuppyDb _puppyDb;
        public CartController(ICart cart, PuppyDb puppyDb) 
        {
            _cart= cart;
            _puppyDb = puppyDb;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_cart.ListCartofUsers());
        }

       /* [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddUserCart([FromBody] AddCartDTO cart)
        {
            bool AlreadyUserHaveCart = _puppyDb.CartTb.Any(c => c.UserId == cart.UserId);
            if (AlreadyUserHaveCart)
            {
                return BadRequest("User Already");
            }
        }*/

    }
}
