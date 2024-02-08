using Puppy_Project.Models.CartDTO;

namespace Puppy_Project.Services.Carts
{
    public interface ICartService
    {
        List<outCartDTO> ListCartofUsers(int id);
        bool CreateUserCart(AddCartDTO user);
        bool RemoveFromUserCart(int id);
        bool UserCartQtyIncrement(int id);
        bool UserCartQtyDecrement(int id);
    }
}
