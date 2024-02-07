using Puppy_Project.Models.Order;

namespace Puppy_Project.Services.Order
{
    public interface IOrder
    {
        List<outOrderDTO> ListUserOrder(int id);
        bool AddUserOrder(inputOrderDTO order);
    }
}
