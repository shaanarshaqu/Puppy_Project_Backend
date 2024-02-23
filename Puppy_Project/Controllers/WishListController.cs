using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.WishListDTO;
using Puppy_Project.Secure;
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


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWishListOfUser()
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int id = TokenDecoder.DecodeToken(token);
            if (id == -1)
            {
                return BadRequest();
            }
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveWishList([FromBody] DeleteFromWishList wish)
        {
            string Bearer_token = HttpContext.Request.Headers["Authorization"];
            string token = Bearer_token.Split(' ')[1];
            int userid = TokenDecoder.DecodeToken(token);
            if (userid == -1)
            {
                return BadRequest();
            }
            bool isDeleted = await _wishList.RemoveWishList(userid, wish.Product_Id);
            return isDeleted ? Ok(isDeleted):BadRequest(isDeleted);
        }
    }
}
