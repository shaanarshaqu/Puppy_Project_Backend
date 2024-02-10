using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetWishListOfUser(int id)
        {
            var list = await _wishList.ListUserWishList(id);
            return Ok(list);
        }


        [HttpPost("AddWishList")]
        [Authorize]
        public async Task<IActionResult> AddWishList([FromBody] AddWishListDTO wish)
        {
            bool isAdded = await _wishList.AddNewWishList(wish);
            return isAdded ? Ok(isAdded):BadRequest(isAdded);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveWishList(int id)
        {
            bool isDeleted = await _wishList.RemoveWishList(id);
            return isDeleted ? Ok(isDeleted):BadRequest(isDeleted);
        }
    }
}
