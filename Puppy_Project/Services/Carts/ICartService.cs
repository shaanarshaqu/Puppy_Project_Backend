using Puppy_Project.Models.CartDTO;

namespace Puppy_Project.Services.Carts
{
    public interface ICartService
    {
        Task<List<outCartDTO>> ListCartofUsers(int id);
        Task<bool> CreateUserCart(AddCartDTO user);
        Task<bool> RemoveFromUserCart(int id);
        Task<bool> UserCartQtyIncrement(int id);
        Task<bool> UserCartQtyDecrement(int id);
    }
}
