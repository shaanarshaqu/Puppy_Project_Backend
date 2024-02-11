using Puppy_Project.Models;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project.Services.Products
{
    public interface IProductsService
    {
        Task<List<outProductDTO>> GetProducts();
        Task<List<outProductDTO>> GetProductsByPage(int pageNo, int pageSize);
        Task<List<outProductDTO>> GetProductsByCategory(int ctg_id);
        Task<outProductDTO> GetProductById(int id);
        Task<bool> AddProduct(AddProductDTO product, IFormFile image);
        Task<bool> UpdateProduct(int id, AddProductDTO product, IFormFile image);
        Task<bool> DeleteProduct(int id);
    }
}
