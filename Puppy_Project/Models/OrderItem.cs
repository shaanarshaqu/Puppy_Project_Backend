
namespace Puppy_Project.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Order_Id { get; set; }
        public Order order { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }
    }
}
