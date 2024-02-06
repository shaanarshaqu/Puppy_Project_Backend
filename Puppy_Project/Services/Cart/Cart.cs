using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.Cart;

namespace Puppy_Project.Services.Cart
{
    public class Cart: ICart
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public Cart(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper = mapper;
        }

        public List<outCartDTO> ListCartofUsers(int id)
        {
            var isUserExist = _puppyDb.UsersTb.Find(id);
            if (isUserExist == null)
            {
                return null;
            }
            var temp = _puppyDb.UsersTb
                .Include(c => c.cartuser)
                    .ThenInclude(ci => ci.cartItemDTOs)
                        .ThenInclude(p => p.product)
                            .ThenInclude(p=>p.Category)
                .FirstOrDefault(c => c.Id == id);
            if(temp == null)
            {
                return null;
            }
            var cartItems = temp.cartuser.cartItemDTOs.Select(t =>
                new outCartDTO
                {
                    Id = t.Id,
                    Type = t.product.Type,
                    Img = t.product.Img,
                    Name = t.product.Name,
                    Detail = t.product.Detail,
                    About = t.product.About,
                    Price = t.product.Price,
                    Qty = t.Qty,
                    Category = t.product.Category.Ctg
                }
            ).ToList();
            return cartItems;
        }

        public bool CreateUserCart(AddCartDTO cartitem)
        {
            bool isUserValid = _puppyDb.UsersTb.Any(u => u.Id == cartitem.UserId);
            bool isProductIdValid = _puppyDb.ProductsTb.Any(p => p.Id == cartitem.Product_Id);
            if (!isUserValid || !isProductIdValid)
            {
                return false;
            }
            var isUserHasCart = _puppyDb.CartTb.SingleOrDefault(c => c.UserId == cartitem.UserId);
            if (isUserHasCart==null)
            {
                _puppyDb.CartTb.Add(new CartDTO { UserId = cartitem.UserId, cartItemDTOs = new List<CartItemDTO>()});
                _puppyDb.SaveChanges();
                isUserHasCart= _puppyDb.CartTb.SingleOrDefault(c => c.UserId == cartitem.UserId);
            }
            var item = _puppyDb.CartItemTb.SingleOrDefault(ci => ci.Product_Id == cartitem.Product_Id && ci.Cart_id == isUserHasCart.Id);
            if (item==null)
            {
                _puppyDb.CartItemTb.Add(
                new CartItemDTO
                {
                    Cart_id = isUserHasCart.Id,
                    Product_Id = cartitem.Product_Id
                });
                _puppyDb.SaveChanges();
                return isUserValid;
            }
            item.Qty++;
            _puppyDb.SaveChanges();
            return isUserValid;
        }
    }
}
