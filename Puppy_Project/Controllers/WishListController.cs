using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.WishListDTO;
using Puppy_Project.Services.WishLists;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishList;
        public WishListController(IWishListService wishList) 
        {
            _wishList=wishList;
        }


        [HttpGet("{id:int}")]
        public IActionResult GetWishListOfUser(int id)
        {
            var list = _wishList.ListUserWishList(id);
            return Ok(list);
        }


        [HttpPost("AddWishList")]
        public IActionResult AddWishList([FromBody] AddWishListDTO wish)
        {
            bool isAdded = _wishList.AddNewWishList(wish);
            return isAdded ? Ok(isAdded):BadRequest(isAdded);
        }

        [HttpDelete("{id:int}")]
        public IActionResult RemoveWishList(int id)
        {
            bool isDeleted = _wishList.RemoveWishList(id);
            return isDeleted ? Ok(isDeleted):BadRequest(isDeleted);
        }
    }
}
