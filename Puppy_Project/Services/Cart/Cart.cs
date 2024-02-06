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
            var temp = _puppyDb.CartTb
                .Include(c=>c.cartItemDTOs)
                    .ThenInclude(ci=>ci.product).Where(c => c.UserId == id).ToList();
            var cartItems = temp.Select(t => 
               new outCartDTO
               {
                   Id = t.cartItemDTOs.First().Id,
                   Type = t.cartItemDTOs.First().product.Type,
                   Img = t.cartItemDTOs.First().product.Img,
                   Name = t.cartItemDTOs.First().product.Name,
                   Detail = t.cartItemDTOs.First().product.Detail,
                   About = t.cartItemDTOs.First().product.About,
                   Price = t.cartItemDTOs.First().product.Price,
                   Qty=t.cartItemDTOs.First().product.Qty,
                   Category = t.cartItemDTOs.First().product.Category.Ctg
               }
            ).ToList();

            /*var list = _mapper.Map<List<outCartDTO>>(_puppyDb.CartTb.ToList());*/
            return cartItems;
        }

        public bool CreateUserCart(AddCartDTO user)
        {
            bool isUserValid = _puppyDb.UsersTb.Any(u => u.Id == user.UserId);
            if (!isUserValid)
            {
                return false;
            }
            var cartDto = _mapper.Map<CartDTO>(user);
            _puppyDb.CartTb.Add(cartDto);
            return isUserValid;
        }
    }
}
