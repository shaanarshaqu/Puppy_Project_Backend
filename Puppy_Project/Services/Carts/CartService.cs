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
        private readonly IConfiguration _configuration;
        public CartService(PuppyDb puppyDb,IMapper mapper, IConfiguration configuration) 
        {
            _puppyDb= puppyDb;
            _mapper = mapper;
            _configuration= configuration;
        }

        public async Task<List<outCartDTO>> ListCartofUsers(int id)
        {
            try
            {
                var isUserExist = await _puppyDb.UsersTb.FindAsync(id);

                if (isUserExist == null)
                {
                    return new List<outCartDTO>();
                }

                var temp = await _puppyDb.UsersTb
                    .Include(c => c.cartuser)
                        .ThenInclude(ci => ci.cartItemDTOs)
                            .ThenInclude(p => p.product)
                                .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (temp.cartuser == null)
                {
                    return new List<outCartDTO>();
                }

                var cartItems = temp.cartuser.cartItemDTOs.Select(t =>
                    new outCartDTO
                    {
                        Id = t.Id,
                        Product_id = t.Product_Id,
                        Type = t.product.Type,
                        Img = $"{_configuration["HostUrl:images"]}/Products/{t.product.Img}",
                        Name = t.product.Name,
                        Detail = t.product.Detail,
                        About = t.product.About,
                        Price = t.product.Price,
                        Qty = t.Qty,
                        Total = (t.Qty * t.product.Price),
                        Category = t.product.Category.Ctg
                    }
                );

                if (cartItems == null)
                {
                    return new List<outCartDTO>();
                }

                return cartItems.ToList();
            }catch(Exception ex)
            {
                return new List<outCartDTO>();
            }
            
        }

        public async Task<bool> CreateUserCart(AddCartDTO cartitem)
        {
            try
            {
                bool isUserValid = await _puppyDb.UsersTb.AnyAsync(u => u.Id == cartitem.UserId);
                bool isProductIdValid = await _puppyDb.ProductsTb.AnyAsync(p => p.Id == cartitem.Product_Id);

                if (!isUserValid || !isProductIdValid)
                {
                    return false;
                }

                var isUserHasCart = await _puppyDb.CartTb.SingleOrDefaultAsync(c => c.UserId == cartitem.UserId);

                if (isUserHasCart == null)
                {
                    _puppyDb.CartTb.Add(new Cart { UserId = cartitem.UserId, cartItemDTOs = new List<CartItem>() });
                    await _puppyDb.SaveChangesAsync();
                    isUserHasCart = await _puppyDb.CartTb.SingleOrDefaultAsync(c => c.UserId == cartitem.UserId);
                }

                var item = await _puppyDb.CartItemTb.SingleOrDefaultAsync(ci => ci.Product_Id == cartitem.Product_Id && ci.Cart_id == isUserHasCart.Id);

                if (item == null)
                {
                    _puppyDb.CartItemTb.Add(
                    new CartItem
                    {
                        Cart_id = isUserHasCart.Id,
                        Product_Id = cartitem.Product_Id
                    });
                    await _puppyDb.SaveChangesAsync();
                    return isUserValid;
                }

                item.Qty++;
                item.Total = item.Qty * item.product.Price;
                await _puppyDb.SaveChangesAsync();
                return isUserValid;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> RemoveFromUserCart(int id)
        {
            try
            {
                var item = await _puppyDb.CartItemTb.FindAsync(id);
                if (item == null)
                {
                    return false;
                }
                _puppyDb.CartItemTb.Remove(item);
                _puppyDb.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> UserCartQtyIncrement(int id)
        {
            try
            {
                var itemInCart = await _puppyDb.CartItemTb.FindAsync(id);
                if (itemInCart == null)
                {
                    return false;
                }
                itemInCart.Qty++;
                await _puppyDb.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> UserCartQtyDecrement(int id)
        {
            try
            {
                var itemInCart = await _puppyDb.CartItemTb.FindAsync(id);
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
                    await _puppyDb.SaveChangesAsync();
                }
                return true;
            }catch( Exception ex)
            {
                return false;
            }
            
        }
    }
}
