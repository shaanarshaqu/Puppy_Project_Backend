using Puppy_Project.Models.OrderDTO;

namespace Puppy_Project.Services.Orders
{
    public interface IOrderService
    {
        Task<List<outOrderDTO>> ListUserOrder(int id);
        Task<bool> AddUserOrder(inputOrderDTO order);
        Task<bool> RemoveAllorders(int userid);
        Task<int> TotalPurchase();
        Task<string> TotalRevenueGenerated();
    }
}
