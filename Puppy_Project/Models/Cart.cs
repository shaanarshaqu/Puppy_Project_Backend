using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public User userid { get; set; }
        public List<CartItem> cartItemDTOs { get; set; }
    }
}
