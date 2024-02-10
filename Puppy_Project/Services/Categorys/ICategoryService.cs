using Puppy_Project.Models;
using Puppy_Project.Models.CategoryDTO;

namespace Puppy_Project.Services.Categorys
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(AddCategoryDTO ctg);
        Task<List<outCategoryDTO>> DisplayCategories();
        Task<bool> DeleteCategory(int id);
    }
}
