namespace Puppy_Project.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Cart_id {  get; set; }
        public Cart cart { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
        public int Qty { get; set; }
        public int Total {  get; set; }
    }
}
