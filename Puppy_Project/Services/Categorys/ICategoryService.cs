using Puppy_Project.Models;
using Puppy_Project.Models.CategoryDTO;

namespace Puppy_Project.Services.Categorys
{
    public interface ICategoryService
    {
        bool AddCategory(AddCategoryDTO ctg);
        List<outCategoryDTO> DisplayCategories();
        bool DeleteCategory(int id);
    }
}
