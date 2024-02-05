namespace Puppy_Project.Models
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int Cart_id {  get; set; }
        public CartDTO cart { get; set; }
        public int Product_Id { get; set; }
        public ProductDTO product { get; set; }
    }
}
