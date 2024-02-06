using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string About { get; set; }
        public int Price { get; set; }
        public int Category_id { get; set; }
        public CategoryDTO Category { get; set; }
        public List<CartItemDTO> cartItems { get; set; }

    }
}
