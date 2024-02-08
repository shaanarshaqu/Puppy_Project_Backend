namespace Puppy_Project.Models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public int WishList_Id { get; set; }
        public WishList wishList { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
    }
}
