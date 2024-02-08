
namespace Puppy_Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string About { get; set; }
        public int Price { get; set; }
        public int Category_id { get; set; }
        public Category Category { get; set; }
        public List<CartItem> cartItems { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public List<WishListItem> wishListitems { get; set; }

    }
}
