using AutoMapper;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.Category;

namespace Puppy_Project.Services.Category
{
    public class Category:ICategory
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public Category(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper = mapper;
        }



        public List<CategoryDTO> DisplayCategories()
        {
            return _puppyDb.CategoryTB.ToList();
        }




        public bool AddCategory(AddCategoryDTO ctg)
        {
            try
            {
                var categoryDto = _mapper.Map<CategoryDTO>(ctg);
                var isCatodoryExist = _puppyDb.CategoryTB.FirstOrDefault(x => x.Ctg.ToLower() == categoryDto.Ctg.ToLower());
                if(isCatodoryExist != null)
                {
                    return false;
                }
                _puppyDb.CategoryTB.Add(categoryDto);
                _puppyDb.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {

            _puppyDb.CategoryTB.Remove();
        }
    }
}
