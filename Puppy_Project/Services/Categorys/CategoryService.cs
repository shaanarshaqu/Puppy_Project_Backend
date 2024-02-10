using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.CategoryDTO;


namespace Puppy_Project.Services.Categorys
{
    public class CategoryService:ICategoryService
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public CategoryService(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper = mapper;
        }



        public async Task<List<outCategoryDTO>> DisplayCategories()
        {
            var list = await _puppyDb.CategoryTB.ToListAsync();
            return _mapper.Map<List<outCategoryDTO>>(list); ;
        }




        public async Task<bool> AddCategory(AddCategoryDTO ctg)
        {
            try
            {
                var categoryDto = _mapper.Map<Models.Category>(ctg);
                var isCatodoryExist =await _puppyDb.CategoryTB.FirstOrDefaultAsync(x => x.Ctg.ToLower() == categoryDto.Ctg.ToLower());
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

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var ctg_to_delete = await _puppyDb.CategoryTB.FirstOrDefaultAsync(x => x.Id == id);
                if (ctg_to_delete == null) 
                {
                    return false;
                }
                _puppyDb.CategoryTB.Remove(ctg_to_delete);
                _puppyDb.SaveChanges();
                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }
    }
}
