using Puppy_Project.Models.WishListDTO;

namespace Puppy_Project.Services.WishLists
{
    public interface IWishListService
    {
        Task<List<outWishListDTO>> ListUserWishList(int userid);
        Task<bool> AddNewWishList(AddWishListDTO wishlist);
        Task<bool> RemoveWishList(int id);
    }
}
