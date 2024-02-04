using Puppy_Project.Models;
using Puppy_Project.Models.Category;

namespace Puppy_Project.Services.Category
{
    public interface ICategory
    {
        bool AddCategory(AddCategoryDTO ctg);
        List<outCategoryDTO> DisplayCategories();
        bool DeleteCategory(int id);
    }
}
