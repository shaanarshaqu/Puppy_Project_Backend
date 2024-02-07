using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int User_Id { get; set; }    
        public UserDTO user { get; set; }
        public List<OrderItemDTO> orderItems { get; set; }
    }
}
