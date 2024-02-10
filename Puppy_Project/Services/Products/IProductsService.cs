using Puppy_Project.Models;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project.Services.Products
{
    public interface IProductsService
    {
        Task<List<outProductDTO>> GetProducts();
        Task<List<outProductDTO>> GetProductsByPage(int pageNo, int pageSize);
        Task<bool> AddProduct(AddProductDTO product, IFormFile image);
        Task<bool> UpdateProduct(int id, AddProductDTO product, IFormFile image);
        Task<bool> DeleteProduct(int id);
    }
}
