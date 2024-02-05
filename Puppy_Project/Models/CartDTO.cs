using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public UserDTO userid { get; set; }
        public List<CartItemDTO> cartItemDTOs { get; set; }
    }
}
