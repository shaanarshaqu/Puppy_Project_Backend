namespace Puppy_Project.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Ctg { get; set; }
        public List<Product> Products { get; set; }
    }
}
