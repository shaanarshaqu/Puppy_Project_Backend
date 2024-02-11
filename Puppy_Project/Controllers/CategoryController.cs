using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.CategoryDTO;
using Puppy_Project.Services.Categorys;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _category;
        public CategoryController(ICategoryService category) 
        { 
            _category = category;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetCategories() 
        { 
            var categorylist = await _category.DisplayCategories();
            return Ok(categorylist);
        }



        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO category) 
        {
            try
            {
                bool isAdded = await _category.AddCategory(category);
                if (!isAdded) 
                {
                    return BadRequest();
                }
                return Ok(category);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                bool isDeleted = await _category.DeleteCategory(id);
                return isDeleted ? Ok(isDeleted) : BadRequest(isDeleted);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
