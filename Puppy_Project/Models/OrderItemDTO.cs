using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int Order_Id { get; set; }
        public OrderDTO order { get; set; }
        public int Product_Id { get; set; }
        public ProductDTO product { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }
    }
}
