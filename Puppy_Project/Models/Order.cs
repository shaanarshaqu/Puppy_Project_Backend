using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int User_Id { get; set; }    
        public User user { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }
}
