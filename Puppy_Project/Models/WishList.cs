using Puppy_Project.Depandancies;

namespace Puppy_Project.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public List<WishListItem> wishListItems { get; set; }
    }
}
