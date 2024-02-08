using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.CartDTO;


namespace Puppy_Project.Services.Carts
{
    public class CartService: ICartService
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public CartService(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper = mapper;
        }

        public List<outCartDTO> ListCartofUsers(int id)
        {
            var isUserExist = _puppyDb.UsersTb.Find(id);
            if (isUserExist == null)
            {
                return new List<outCartDTO>();
            }
            var temp = _puppyDb.UsersTb
                .Include(c => c.cartuser)
                    .ThenInclude(ci => ci.cartItemDTOs)
                        .ThenInclude(p => p.product)
                            .ThenInclude(p=>p.Category)
                .FirstOrDefault(c => c.Id == id);
            if(temp.cartuser == null)
            {
                return new List<outCartDTO>();
            }
            var cartItems = temp.cartuser.cartItemDTOs.Select(t =>
                new outCartDTO
                {
                    Id = t.Id,
                    Product_id=t.Product_Id,
                    Type = t.product.Type,
                    Img = t.product.Img,
                    Name = t.product.Name,
                    Detail = t.product.Detail,
                    About = t.product.About,
                    Price = t.product.Price,
                    Qty = t.Qty,
                    Total=(t.Qty * t.product.Price),
                    Category = t.product.Category.Ctg
                }
            );
            if (cartItems == null)
            {
                return new List<outCartDTO>();
            }
            return cartItems.ToList();
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
                _puppyDb.CartTb.Add(new Cart { UserId = cartitem.UserId, cartItemDTOs = new List<CartItem>()});
                _puppyDb.SaveChanges();
                isUserHasCart= _puppyDb.CartTb.SingleOrDefault(c => c.UserId == cartitem.UserId);
            }
            var item = _puppyDb.CartItemTb.SingleOrDefault(ci => ci.Product_Id == cartitem.Product_Id && ci.Cart_id == isUserHasCart.Id);
            if (item==null)
            {
                _puppyDb.CartItemTb.Add(
                new CartItem
                {
                    Cart_id = isUserHasCart.Id,
                    Product_Id = cartitem.Product_Id
                });
                _puppyDb.SaveChanges();
                return isUserValid;
            }
            item.Qty++;
            item.Total = item.Qty * item.product.Price;
            _puppyDb.SaveChanges();
            return isUserValid;
        }

        public bool RemoveFromUserCart(int id)
        {
            var item = _puppyDb.CartItemTb.Find(id);
            if (item == null)
            {
                return false;
            }
            _puppyDb.CartItemTb.Remove(item);
            _puppyDb.SaveChanges();
            return true;
        }

        public bool UserCartQtyIncrement(int id)
        {
            var itemInCart = _puppyDb.CartItemTb.Find(id);
            if (itemInCart == null)
            {
                return false;
            }
            itemInCart.Qty++;
            _puppyDb.SaveChanges();
            return true;
        }

        public bool UserCartQtyDecrement(int id)
        {
            var itemInCart = _puppyDb.CartItemTb.Find(id);
            if (itemInCart == null)
            {
                return false;
            }
            if (itemInCart.Qty == 1)
            {
                return false;
            }
                if (itemInCart.Qty > 1)
            {
                itemInCart.Qty--;
                _puppyDb.SaveChanges();
            }
            return true;
        }
    }
}
