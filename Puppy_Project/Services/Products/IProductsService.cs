using Puppy_Project.Models;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project.Services.Products
{
    public interface IProductsService
    {
        List<outProductDTO> GetProducts();
        bool AddProduct(AddProductDTO product);
        bool UpdateProduct(int id, AddProductDTO product);
        bool DeleteProduct(int id);
    }
}
