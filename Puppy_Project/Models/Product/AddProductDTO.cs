using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Models.Product
{
    public class AddProductDTO
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public string Name { get; set; }
        public string Detail { get; set; }
        public string About { get; set; }
        [Required]
        public int Price { get; set; }
        public int Qty { get; set; }
        [Required]
        public int Category_id { get; set; }
    }
}
