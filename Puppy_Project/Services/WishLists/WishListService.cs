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
        private  readonly IConfiguration _configuration;
        public WishListService(PuppyDb puppyDb, IConfiguration configuration) 
        {
            _puppyDb= puppyDb;
            _configuration= configuration;
        }



        public async Task<List<outWishListDTO>> ListUserWishList(int userid)
        {
            try
            {
                bool isUserHasWishList = await _puppyDb.WishListTb.AnyAsync(w => w.UserId == userid);
                if (!isUserHasWishList)
                {
                    return new List<outWishListDTO>();
                }
                var userwishlist = await _puppyDb.WishListTb
                    .Include(w => w.wishListItems)
                    .ThenInclude(wi => wi.product)
                    .FirstOrDefaultAsync(w => w.UserId == userid);

                if (userwishlist == null)
                {
                    return new List<outWishListDTO>();
                }

                var userwishlistToList = userwishlist.wishListItems.Select(wi =>
                            new outWishListDTO
                            {
                                Id = wi.Id,
                                Img = $"{_configuration["HostUrl:images"]}/Products/{wi.product.Img}",
                                Name = wi.product.Name,
                                Detail = wi.product.Detail,
                                Price = wi.product.Price,
                                Product_id = wi.product.Id
                            }).ToList();

                return userwishlistToList;
            }catch(Exception ex)
            {
                return new List<outWishListDTO>();
            }
            
        }



        public async Task<bool> AddNewWishList(AddWishListDTO wishlist)
        {
            try
            {
                bool isUserValid = await _puppyDb.UsersTb.AnyAsync(u => u.Id == wishlist.User_Id);
                bool isProductValid = await _puppyDb.ProductsTb.AnyAsync(p => p.Id == wishlist.Product_Id);
                if (!isUserValid || !isProductValid)
                {
                    return false;
                }
                var isUserHasWishList = await _puppyDb.WishListTb.SingleOrDefaultAsync(w => w.UserId == wishlist.User_Id);
                if (isUserHasWishList == null)
                {
                    _puppyDb.WishListTb.Add(new WishList { UserId = wishlist.User_Id });
                    await _puppyDb.SaveChangesAsync();
                    isUserHasWishList = await _puppyDb.WishListTb.SingleOrDefaultAsync(w => w.UserId == wishlist.User_Id);
                }
                var isProductAlreadyinWishlist = await _puppyDb.WishListItemTb.SingleOrDefaultAsync(wi => wi.WishList_Id == isUserHasWishList.Id && wi.Product_Id == wishlist.Product_Id);
                if (isProductAlreadyinWishlist == null)
                {
                    _puppyDb.WishListItemTb.Add(new WishListItem { WishList_Id = isUserHasWishList.Id, Product_Id = wishlist.Product_Id });
                    await _puppyDb.SaveChangesAsync();
                    return true;
                }
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> RemoveWishList(int userid, int productid)
        {
            try
            {
                var IsWishListExist = await _puppyDb.WishListTb.SingleOrDefaultAsync(w=>w.UserId == userid);
                if (IsWishListExist == null)
                {
                    return false;
                }
                var IsWishListItemExist = await _puppyDb.WishListItemTb.SingleOrDefaultAsync(wi => wi.WishList_Id == IsWishListExist.Id && wi.Product_Id == productid);
                if (IsWishListItemExist == null)
                {
                    return false;
                }
                _puppyDb.WishListItemTb.Remove(IsWishListItemExist);
                await _puppyDb.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
