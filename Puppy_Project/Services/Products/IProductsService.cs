using Puppy_Project.Models;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project.Services.Products
{
    public interface IProductsService
    {
        List<outProductDTO> GetProducts();
        bool AddProduct(AddProductDTO product, IFormFile image);
        bool UpdateProduct(int id, AddProductDTO product, IFormFile image);
        bool DeleteProduct(int id);
    }
}
