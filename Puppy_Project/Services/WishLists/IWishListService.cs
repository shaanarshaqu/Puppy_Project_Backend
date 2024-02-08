using Puppy_Project.Models.WishListDTO;

namespace Puppy_Project.Services.WishLists
{
    public interface IWishListService
    {
        List<outWishListDTO> ListUserWishList(int userid);
        bool AddNewWishList(AddWishListDTO wishlist);
        bool RemoveWishList(int id);
    }
}
