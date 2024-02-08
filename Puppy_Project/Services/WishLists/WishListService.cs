using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Depandancies;
using Puppy_Project.Models;
using Puppy_Project.Models.WishListDTO;

namespace Puppy_Project.Services.WishLists
{
    public class WishListService: IWishListService
    {
        private readonly PuppyDb _puppyDb;
        public WishListService(PuppyDb puppyDb) 
        {
            _puppyDb= puppyDb;
        }



        public List<outWishListDTO> ListUserWishList(int userid)
        {
            bool isUserHasWishList = _puppyDb.WishListTb.Any(w=>w.UserId == userid);
            if (!isUserHasWishList)
            {
                return new List<outWishListDTO>();
            }
            var userwishlist = _puppyDb.WishListTb
                .Include(w=>w.wishListItems)
                .ThenInclude(wi=>wi.product)
                .FirstOrDefault(w=>w.UserId == userid);

            if(userwishlist == null)
            {
                return new List<outWishListDTO>();
            }

            var userwishlistToList = userwishlist.wishListItems.Select(wi => 
                        new outWishListDTO
                            {
                                Id= wi.Id,
                                Img = wi.product.Img,
                                Name= wi.product.Name,
                                Detail= wi.product.Detail,
                                Price= wi.product.Price,
                                Product_id = wi.product.Id
                            }).ToList();

            return userwishlistToList;
        }



        public bool AddNewWishList(AddWishListDTO wishlist)
        {
            bool isUserValid = _puppyDb.UsersTb.Any(u => u.Id == wishlist.User_Id);
            bool isProductValid = _puppyDb.ProductsTb.Any(p => p.Id == wishlist.Product_Id);
            if (!isUserValid || !isProductValid)
            {
                return false;
            }
            var isUserHasWishList = _puppyDb.WishListTb.SingleOrDefault(w=>w.UserId==wishlist.User_Id);
            if(isUserHasWishList == null)
            {
                _puppyDb.WishListTb.Add(new WishList { UserId = wishlist.User_Id });
                _puppyDb.SaveChanges();
                isUserHasWishList = _puppyDb.WishListTb.SingleOrDefault(w => w.UserId == wishlist.User_Id);
            }
            var isProductAlreadyinWishlist = _puppyDb.WishListItemTb.SingleOrDefault(wi=>wi.WishList_Id == isUserHasWishList.Id && wi.Product_Id == wishlist.Product_Id);
            if( isProductAlreadyinWishlist == null)
            {
                _puppyDb.WishListItemTb.Add(new WishListItem { WishList_Id = isUserHasWishList.Id , Product_Id = wishlist.Product_Id});
                _puppyDb.SaveChanges();
                return true;
            }
            return true;
        }

        public bool RemoveWishList(int id)
        {
            var IsWishListExist = _puppyDb.WishListItemTb.Find(id);
            if( IsWishListExist == null)
            {
                return false;
            }
            _puppyDb.WishListItemTb.Remove(IsWishListExist);
            _puppyDb.SaveChanges();
            return true;
        }
    }
}
