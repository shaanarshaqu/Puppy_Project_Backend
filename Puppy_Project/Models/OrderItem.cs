
namespace Puppy_Project.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Order_Id { get; set; }
        public Order order { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }
        public string DelivaryAddress { get; set; }
        public DateTime OrderDate { get; set; } 
        public string DeliveryStatus { get; set; }
    }
}
