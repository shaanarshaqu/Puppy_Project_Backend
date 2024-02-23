using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Models.RazorPay;

namespace Puppy_Project.Services.Orders
{
    public interface IOrderService
    {
        Task<List<outOrderDTO>> ListUserOrder(int id);
        Task<bool> RemoveAllorders(int userid);
        Task<int> TotalPurchase();
        Task<string> TotalRevenueGenerated();
        Task<string> OrderCreate(long price);
        bool Payment(RazorpayDTO razorpay);
        Task<bool> CreateOrder(inputOrderDTO userDetails);
    }
}
