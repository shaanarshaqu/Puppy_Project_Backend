using Puppy_Project.Models.OrderDTO;

namespace Puppy_Project.Services.Orders
{
    public interface IOrderService
    {
        List<outOrderDTO> ListUserOrder(int id);
        bool AddUserOrder(inputOrderDTO order);
        bool RemoveAllorders(int userid);
    }
}
