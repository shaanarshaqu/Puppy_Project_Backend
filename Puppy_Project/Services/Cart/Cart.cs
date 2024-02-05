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
            var temp = _puppyDb.CartItemTb.Include(c => c.Cart_id)
                .Include(c => c.product);
            var list = _mapper.Map<List<outCartDTO>>(_puppyDb.CartTb.ToList());
            return list;
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
